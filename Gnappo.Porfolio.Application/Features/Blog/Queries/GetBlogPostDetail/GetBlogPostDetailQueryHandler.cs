using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Gnappo.Portfolio.Application.Features.Helpers.ContentTokenReplacer;
using Microsoft.Extensions.Configuration;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using Gnappo.Portfolio.Domain.Models;
using Gnappo.Portfolio.Application.Models;
using Microsoft.Extensions.Options;

namespace Gnappo.Portfolio.Application.Features.Blog.Queries.GetBlogPostDetail
{
    public class GetBlogPostDetailQueryHandler : IRequestHandler<GetBlogPostDetailQuery, BlogPostDetailDto>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly List<TokenReplacer> _tokenReplacers;

        public GetBlogPostDetailQueryHandler(IMapper mapper, IBlobService blobService, IOptions<WebClientSettings> webClientSettings)
        {
            _mapper = mapper;
            _blobService = blobService;

            var storageUrlFormatter = new StorageUrlFormatter(
                webClientSettings.Value.UrlPatterns.FileBinary
                );

            var articleUrlFormatter = new ContentPageUrlFormatter(
                webClientSettings.Value.UrlPatterns.Article,
                webClientSettings.Value.WebPortfolioUrl
                );

            var blogPostUrlFormatter = new ContentPageUrlFormatter(
                webClientSettings.Value.UrlPatterns.BlogPost,
                webClientSettings.Value.WebPortfolioUrl
                );
            var pdfPageUrlFormatter = new ContentPageUrlFormatter(
                webClientSettings.Value.UrlPatterns.Pdf,
                webClientSettings.Value.WebPortfolioUrl
                );

            _tokenReplacers = new List<TokenReplacer>();
            _tokenReplacers.Add(new FileTokenReplacer(webClientSettings.Value.ContentTokens.File, blobService, storageUrlFormatter));
            _tokenReplacers.Add(new ContentPageTokenReplacer(webClientSettings.Value.ContentTokens.Article, articleUrlFormatter));
            _tokenReplacers.Add(new ContentPageTokenReplacer(webClientSettings.Value.ContentTokens.BlogPost, blogPostUrlFormatter));
            _tokenReplacers.Add(new ContentPageTokenReplacer(webClientSettings.Value.ContentTokens.Pdf, pdfPageUrlFormatter));
        }

        public async Task<BlogPostDetailDto> Handle(GetBlogPostDetailQuery request, CancellationToken cancellationToken)
        {
            var post = await _blobService.GetBlogPostDetailAsync(request.Name, cancellationToken);

            foreach (var tokenReplacer in _tokenReplacers)
            {
                post.Content = tokenReplacer.ReplaceTokens(post.Content);
            }

            return _mapper.Map<BlogPostDetailDto>(post);

        }
    }
}
