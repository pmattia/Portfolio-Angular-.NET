using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Domain.common;
using Gnappo.Portfolio.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Contracts.Infrastructure
{
    public interface IBlobService
    {
        string GetAvatarUrl(AvatarEmotion emotion);
        Task<TopicInfo[]> GetMainTopicsAsync(CancellationToken cancellationToken);
        TopicInfo[] GetMainTopics();
        Task<Article[]> GetArticlesAsync(CancellationToken cancellationToken);
        Article[] GetArticles();
        Task<Article[]> FindArticlesByTagsAsync(IEnumerable<string> tags, CancellationToken cancellationToken);
        Article[] FindArticlesByTags(IEnumerable<string> tags);
        Task<Article> FindArticleByDocumentIdAsync(string documentId, CancellationToken cancellationToken);
        Article FindArticleByDocumentIdAsync(string documentId);
        ArticleDetail GetArticleDetail(string articleName);
        Task<ArticleDetail> GetArticleDetailAsync(string articleName, CancellationToken cancellationToken);
        Task<BlogPost[]> GetBlogPostsAsync(CancellationToken cancellationToken);
        BlogPost[] GetBlogPosts();
        Task<BlogPost> FindBlogPostsByDocumentIdAsync(string documentId, CancellationToken cancellationToken);
        BlogPost FindBlogPostsByDocumentId(string documentId);
        BlogPostDetail GetBlogPostDetail(string blogPostName);
        Task<BlogPostDetail> GetBlogPostDetailAsync(string blogPostName, CancellationToken cancellationToken);
        Task<ContentFile> GetContentFileAsync(string fileId, CancellationToken cancellationToken);
        ContentFile GetContentFile(string fileId);
        Task<IEnumerable<SharedFile>> GetFileListAsync(string path, CancellationToken cancellationToken);
        IEnumerable<SharedFile> GetFileList(string path);
        Task<byte[]> GetFileBynaryAsync(string filePath, CancellationToken cancellationToken);
        byte[] GetFileBynary(string filePath);
    }
}
