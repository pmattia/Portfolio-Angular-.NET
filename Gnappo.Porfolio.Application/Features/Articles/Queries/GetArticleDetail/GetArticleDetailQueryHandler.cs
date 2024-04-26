using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Helpers.ContentTokenReplacer;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using Gnappo.Portfolio.Application.Models;
using MediatR;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetArticleDetail
{
    public class GetArticleDetailQueryHandler : IRequestHandler<GetArticleDetailQuery, ArticleDetailDto>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly List<TokenReplacer> _tokenReplacers;

        public GetArticleDetailQueryHandler(IMapper mapper, IBlobService blobService, IOptions<WebClientSettings> webClientSettings)
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

        public async Task<ArticleDetailDto> Handle(GetArticleDetailQuery request, CancellationToken cancellationToken)
        {
            var article = await _blobService.GetArticleDetailAsync(request.Name, cancellationToken);

            foreach(var tokenReplacer in _tokenReplacers)
            {
                article.Content = tokenReplacer.ReplaceTokens(article.Content);
            }   

            return _mapper.Map<ArticleDetailDto>(article);
        }
    }
}
