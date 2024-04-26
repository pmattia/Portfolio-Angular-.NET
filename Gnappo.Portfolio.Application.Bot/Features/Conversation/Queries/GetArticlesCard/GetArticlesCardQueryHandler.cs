using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using System.Collections.Generic;
using Gnappo.Portfolio.Domain.Models;
using Gnappo.Portfolio.Application.Models;
using Microsoft.Extensions.Options;
using Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.AdaptiveCards;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetArticlesCard
{
    public class GetArticlesCardQueryHandler : IRequestHandler<GetArticlesCardQuery, Attachment>
    {
        private readonly IBlobService _blobService;
        private readonly IUrlFormatter _articlePageUrlFormatter;
        private readonly IUrlFormatter _pdfPageUrlFormatter;
        private readonly IUrlFormatter _storageUrlFormatter;

        public GetArticlesCardQueryHandler(IBlobService blobService, IOptions<WebClientSettings> webClientSettings)
        {
            _articlePageUrlFormatter = new ContentPageUrlFormatter(
                webClientSettings.Value.UrlPatterns.Article,
                webClientSettings.Value.WebPortfolioUrl
                );

            _pdfPageUrlFormatter = new ContentPageUrlFormatter(
                webClientSettings.Value.UrlPatterns.Pdf,
                webClientSettings.Value.WebPortfolioUrl
                );

            _storageUrlFormatter = new StorageUrlFormatter(
                webClientSettings.Value.UrlPatterns.FileBinary
                );
            _blobService = blobService;
        }

        public async Task<Attachment> Handle(GetArticlesCardQuery request, CancellationToken cancellationToken)
        {
            var selectedArticles = await _blobService.FindArticlesByTagsAsync(request.Tags, cancellationToken);

            var selectedPdf = new List<ContentFile>();
            foreach (var pdfFileId in request.PdfFileIds)
            {
                var pdfFile = await _blobService.GetContentFileAsync(pdfFileId, cancellationToken);
                if (pdfFile != null)
                {
                    selectedPdf.Add(pdfFile);
                }
            }

            var coverUrl = string.Empty;
            if (!string.IsNullOrWhiteSpace(request.CoverFileId))
            {
                var image = await _blobService.GetContentFileAsync(request.CoverFileId, cancellationToken);
                coverUrl = _storageUrlFormatter.Format(image.relativePath);
            }


            var adaptiveCard = new AdaptiveCardBuilder()
                        .SetCoverUrl(coverUrl)
                        .AddActions(GetArticleCards(selectedArticles))
                        .AddActions(GetPdfCards(selectedPdf))
                        .AddActions(GetAdditionalActionCards(request.AdditionalActions))
                        .Build();

            return new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCard, new JsonSerializerSettings { MaxDepth = null }),
            };
        }


        private IList<AdaptiveCardAction> GetPdfCards(IEnumerable<ContentFile> pdfs)
        {
            return pdfs.Select(a => new AdaptiveCardAction()
            {
                name = a.Id,
                title = a.Name,
                type = "openUrl",
                contentType = PortfolioContentTypeEnum.Pdf,
                url = _pdfPageUrlFormatter.Format(a.Id)
            }).ToList();
        }

        private IList<AdaptiveCardAction> GetAdditionalActionCards(IEnumerable<CardAction> additionalActions)
        {
            return additionalActions.Select(a => new AdaptiveCardAction()
            {
                title = a.Title,
                type = "messageBack",
                data = a.Text
            }).ToList();
        }

        private IList<AdaptiveCardAction> GetArticleCards(IEnumerable<Article> articles)
        {
            return articles.Select(a => new AdaptiveCardAction()
            {
                name = a.Id,
                title = a.Title,
                type = "openUrl",
                contentType = PortfolioContentTypeEnum.Article,
                url = _articlePageUrlFormatter.Format(a.Id)
            }).ToList();
        }
    }
}