using Azure.Storage.Blobs;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Azure.Storage.Files.Shares;
using Gnappo.Portfolio.Application.Features.Storage.Queries;
using Azure.Storage.Files.Shares.Models;
using Gnappo.Portfolio.Domain.common;
using Azure.Core;
using MediatR;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using Gnappo.Portfolio.Infrastructure.Email;
using Microsoft.Extensions.Logging;

namespace Gnappo.Portfolio.Infrastructure.Storage
{
    public class AzureBlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _avatarContainerClient;
        private readonly BlobContainerClient _contentsContainerClient;
        private readonly BlobContainerClient _articlesContainerClient;
        private readonly BlobContainerClient _blogContainerClient;
        private readonly ShareClient _shareClient;
        private readonly ILogger<AzureBlobService> _logger;

        public AzureBlobService(IOptions<AzureStorageSettings> storageSettings, ILogger<AzureBlobService> logger)
        {
            _logger = logger;
            var connectionString = storageSettings.Value.BlobServiceConnectionString;
            var avatarContainerName = storageSettings.Value.BlobContainerNames.BlobAvatarContainerName;
            var contentsContainerName = storageSettings.Value.BlobContainerNames.BlobContentsContainerName;
            var articlesContainerName = storageSettings.Value.BlobContainerNames.BlobArticlesContainerName;
            var blogContainerName = storageSettings.Value.BlobContainerNames.BlobBlogContainerName;

            _blobServiceClient = new BlobServiceClient(connectionString);
            _avatarContainerClient = _blobServiceClient.GetBlobContainerClient(avatarContainerName);
            _contentsContainerClient = _blobServiceClient.GetBlobContainerClient(contentsContainerName);
            _articlesContainerClient = _blobServiceClient.GetBlobContainerClient(articlesContainerName);
            _blogContainerClient = _blobServiceClient.GetBlobContainerClient(blogContainerName);

            _shareClient = new ShareClient(connectionString, storageSettings.Value.FileShareFolderName);
        }

        public string GetAvatarUrl(AvatarEmotion emotion)
        {
            var avatarBlob = _avatarContainerClient.GetBlobClient($"{emotion.ToString().ToLower()}.svg");
            return avatarBlob.Uri.ToString();
        }

        public async Task<TopicInfo[]> GetMainTopicsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await getFromContainerAsync<TopicInfo[]>(_contentsContainerClient, "main-topics.json", cancellationToken);
            }
            catch(FileNotFoundException ex)
            {
                _logger.LogError(ex, "Main topics file not found");
                return null;
            }
        }

        public TopicInfo[] GetMainTopics()
        {
            try { 
                return getFromContainer<TopicInfo[]>(_contentsContainerClient, "main-topics.json");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Main topics file not found");
                return null;
            }
        }

        public async Task<Article[]> GetArticlesAsync(CancellationToken cancellationToken)
        {
            try { 
                var articles = await getFromContainerAsync<Article[]>(_contentsContainerClient, "articles.json", cancellationToken);
                return articles.OrderBy(a => a.Order).ToArray();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Articles file not found");
                return null;
            }
        }

        public Article[] GetArticles()
        {
            try { 
                return getFromContainer<Article[]>(_contentsContainerClient, "articles.json")
                    .OrderBy(a => a.Order)
                    .ToArray();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Articles file not found");
                return null;
            }
        }

        public async Task<Article[]> FindArticlesByTagsAsync(IEnumerable<string> tags, CancellationToken cancellationToken)
        {
            try
            {
                var articles = await getFromContainerAsync<Article[]>(_contentsContainerClient, "articles.json", cancellationToken); 
                return articles.Where(a => tags.Count() == 0 || tags.Intersect(a.Tags).Count() > 0 && a.Published)
                    .OrderBy(a => a.Order)
                    .ToArray();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Articles file not found");
                return null;
            }
        }

        public Article[] FindArticlesByTags(IEnumerable<string> tags)
        {
            try
            {
                var articles = getFromContainer<Article[]>(_contentsContainerClient, "articles.json");
                return articles.Where(a => tags.Count() == 0 || tags.Intersect(a.Tags).Count() > 0 && a.Published)
                    .OrderBy(a => a.Order)
                    .ToArray();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Articles file not found");
                return null;
            }
        }

        public async Task<Article> FindArticleByDocumentIdAsync(string documentId, CancellationToken cancellationToken)
        {
            try
            {
                var articles = await getFromContainerAsync<Article[]>(_contentsContainerClient, "articles.json", cancellationToken);
                return articles.FirstOrDefault(article => !string.IsNullOrEmpty(article.AssistantFileId) && article.AssistantFileId == documentId);    
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Articles file not found");
                return null;
            }
        }

        public Article FindArticleByDocumentIdAsync(string documentId)
        {
            try
            {
                var articles = getFromContainer<Article[]>(_contentsContainerClient, "articles.json");
                return articles.FirstOrDefault(article => !string.IsNullOrEmpty(article.AssistantFileId) && article.AssistantFileId == documentId);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Articles file not found");
                return null;
            }
        }

        public async Task<ArticleDetail> GetArticleDetailAsync(string articleName, CancellationToken cancellationToken)
        {
            try { 
                return await getFromContainerAsync<ArticleDetail>(_articlesContainerClient, $"{articleName}.json", cancellationToken);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Article detail file not found");
                return null;
            }
        }

        public ArticleDetail GetArticleDetail(string articleName)
        {
            try { 
                return getFromContainer<ArticleDetail>(_articlesContainerClient, $"{articleName}.json");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Article detail file not found");
                return null;
            }
        }

        public async Task<BlogPost[]> GetBlogPostsAsync(CancellationToken cancellationToken)
        {
            try { 
                var posts = await getFromContainerAsync<BlogPost[]>(_contentsContainerClient, "blog-posts.json", cancellationToken);
                return posts.OrderBy(p => p.Order).ToArray();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Blog posts file not found");
                return null;
            }
        }

        public BlogPost[] GetBlogPosts()
        {
            try { 
                return getFromContainer<BlogPost[]>(_contentsContainerClient, "blog-posts.json").OrderBy(p => p.Order).ToArray();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Blog posts file not found");
                return null;
            }
        }

        public async Task<BlogPost> FindBlogPostsByDocumentIdAsync(string documentId, CancellationToken cancellationToken)
        {
            try
            {
                var blogPosts = await getFromContainerAsync<BlogPost[]>(_contentsContainerClient, "blog-posts.json", cancellationToken);
                return blogPosts.FirstOrDefault(blogPost => !string.IsNullOrEmpty(blogPost.AssistantFileId) && blogPost.AssistantFileId == documentId);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Blog posts file not found");
                return null;
            }
        }

        public BlogPost FindBlogPostsByDocumentId(string documentId)
        {
            try
            {
                var blogPosts = getFromContainer<BlogPost[]>(_contentsContainerClient, "blog-posts.json");
                return blogPosts.FirstOrDefault(blogPost => !string.IsNullOrEmpty(blogPost.AssistantFileId) && blogPost.AssistantFileId == documentId);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Blog posts file not found");
                return null;
            }
        }

        public BlogPostDetail GetBlogPostDetail(string postName)
        {
            try { 
                return getFromContainer<BlogPostDetail>(_blogContainerClient, $"{postName}.json");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Blog post detail file not found");
                return null;
            }
        }

        public async Task<BlogPostDetail> GetBlogPostDetailAsync(string postName, CancellationToken cancellationToken)
        {
            try { 
                return await getFromContainerAsync<BlogPostDetail>(_blogContainerClient, $"{postName}.json", cancellationToken);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Blog post detail file not found");
                return null;
            }
        }

        public async Task<ContentFile> GetContentFileAsync(string fileId, CancellationToken cancellationToken)
        {
            try
            {
                var files = await getFromContainerAsync<ContentFile[]>(_contentsContainerClient, "shared-files.json", cancellationToken);
                return files.FirstOrDefault(file => file.Id == fileId);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Shared files file not found");
                return null;
            }
        }

        public ContentFile GetContentFile(string fileId)
        {
            try
            {
                var files = getFromContainer<ContentFile[]>(_contentsContainerClient, "shared-files.json");
                return files.FirstOrDefault(file => file.Id == fileId);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Shared files file not found");
                return null;
            }
        }

        public async Task<IEnumerable<SharedFile>> GetFileListAsync(string path, CancellationToken cancellationToken)
        {
            try
            {
                var directoryClient = _shareClient.GetDirectoryClient(path);
                var response = new List<SharedFile>();
                await foreach (ShareFileItem fileItem in directoryClient.GetFilesAndDirectoriesAsync())
                {
                    response.Add(new SharedFile()
                    {
                        Name = fileItem.Name
                    });
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file list");
                return null;
            }
        }

        public IEnumerable<SharedFile> GetFileList(string path)
        {
            try
            {
                var directoryClient = _shareClient.GetDirectoryClient(path);
                var response = new List<SharedFile>();
                foreach (ShareFileItem fileItem in directoryClient.GetFilesAndDirectories())
                {
                    response.Add(new SharedFile()
                    {
                        Name = fileItem.Name
                    });
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file list");
                return null;
            }
        }

        public async Task<byte[]> GetFileBynaryAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                var directoryClient = _shareClient.GetDirectoryClient(Path.GetDirectoryName(filePath));
                var fileClient = directoryClient.GetFileClient(Path.GetFileName(filePath));
                var response = await fileClient.DownloadAsync(null,cancellationToken);
                using var memoryStream = new MemoryStream();
                await response.Value.Content.CopyToAsync(memoryStream, cancellationToken);
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                _logger .LogError(ex, "Error getting file binary");
                return null;
            }
        }

        public byte[] GetFileBynary(string filePath)
        {
            try
            {
                var directoryClient = _shareClient.GetDirectoryClient(Path.GetDirectoryName(filePath));
                var fileClient = directoryClient.GetFileClient(Path.GetFileName(filePath));
                var response = fileClient.Download();
                using var memoryStream = new MemoryStream();
                response.Value.Content.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file binary");
                return null;
            }
        }

        private T getFromContainer<T>(BlobContainerClient container, string blobName)
        {
            var blob = container.GetBlobClient(blobName);
            var exist = blob.Exists();
            if (!exist) throw new FileNotFoundException();

            using var stream = blob.OpenRead(null);
            using var streamReader = new StreamReader(stream);
            using var json = new JsonTextReader(streamReader);
            return JsonSerializer.CreateDefault().Deserialize<T>(json);
        }

        private async Task<T> getFromContainerAsync<T>(BlobContainerClient container, string blobName, CancellationToken cancellationToken)
        {
            var blob = container.GetBlobClient(blobName);
            var exist =await blob.ExistsAsync(cancellationToken);
            if (!exist) throw new FileNotFoundException();

            using var stream = await blob.OpenReadAsync(null, cancellationToken);
            using var streamReader = new StreamReader(stream);
            using var json = new JsonTextReader(streamReader);
            return JsonSerializer.CreateDefault().Deserialize<T>(json);
        }
    }
}
