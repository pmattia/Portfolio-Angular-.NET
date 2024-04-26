using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using MediatR;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Gnappo.Portfolio.Application.Contracts.Domain;
using System.Collections.Generic;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using Gnappo.Portfolio.Application.Models;
using Microsoft.Extensions.Options;
using Gnappo.Portfolio.Application.Bot.Resources;
using Microsoft.Extensions.Localization;
using Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.AdaptiveCards;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetContactsCard
{
    public class GetContactsCardQueryHandler : IRequestHandler<GetContactsCardQuery, Attachment>
    {
        private readonly string _contactUrlPattern;
        private readonly string _webPortfolioUrl;
        private readonly string _linkedinUrl;
        private readonly string _cvPdfFileId;
        private readonly string _mailMeText;
        private readonly string _reachMeOnLinkedinText;
        private readonly IUrlFormatter _storageUrlFormatter;
        private readonly IUrlFormatter _pdfPageUrlFormatter;
        private readonly IBlobService _blobService;

        public GetContactsCardQueryHandler(IStringLocalizer<ConversationResources> localizer, IBlobService blobService, IOptions<WebClientSettings> webClientSettings)
        {
            _contactUrlPattern = webClientSettings.Value.UrlPatterns.Contact;
            _webPortfolioUrl = webClientSettings.Value.WebPortfolioUrl;
            _linkedinUrl = webClientSettings.Value.LinkedinUrl;
            _cvPdfFileId = webClientSettings.Value.CvPdfFileId;

            _mailMeText = localizer["MailMe"];
            _reachMeOnLinkedinText = localizer["ReachMeOnLinkedin"];

            _pdfPageUrlFormatter = new ContentPageUrlFormatter(
                webClientSettings.Value.UrlPatterns.Pdf,
                webClientSettings.Value.WebPortfolioUrl
                );
            _storageUrlFormatter = new StorageUrlFormatter(
                webClientSettings.Value.UrlPatterns.FileBinary
                );
            _blobService = blobService;
        }

        public async Task<Attachment> Handle(GetContactsCardQuery request, CancellationToken cancellationToken)
        {
            var actions = new List<AdaptiveCardAction>();

            actions.Add(new AdaptiveCardAction()
            {
                name = _mailMeText,
                title = _mailMeText,
                type = "openUrl",
                contentType = PortfolioContentTypeEnum.Contact,
                url = string.Format(_contactUrlPattern, _webPortfolioUrl)
            });

            if (!string.IsNullOrWhiteSpace(_linkedinUrl))
            {
                actions.Add(new AdaptiveCardAction()
                {
                    name = _reachMeOnLinkedinText,
                    title = _reachMeOnLinkedinText,
                    type = "openUrl",
                    contentType = PortfolioContentTypeEnum.ContentUrl,
                    url = _linkedinUrl
                });
            }

            if (!string.IsNullOrWhiteSpace(_cvPdfFileId))
            {
                var pdfFile = await _blobService.GetContentFileAsync(_cvPdfFileId, cancellationToken);
                actions.Add(new AdaptiveCardAction()
                {
                    name = pdfFile.Id,
                    title = pdfFile.Name,
                    type = "openUrl",
                    contentType = PortfolioContentTypeEnum.Pdf,
                    url = _pdfPageUrlFormatter.Format(pdfFile.Id)
                });
            }

            var coverUrl = string.Empty;
            if (!string.IsNullOrWhiteSpace(request.CoverFileId))
            {
                var image = await _blobService.GetContentFileAsync(request.CoverFileId, cancellationToken);
                coverUrl = _storageUrlFormatter.Format(image.relativePath);
            }

            var adaptiveCard = new AdaptiveCardBuilder()
                        .SetCoverUrl(coverUrl)
                        .AddActions(actions)
                        .Build();

            return new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCard, new JsonSerializerSettings { MaxDepth = null }),
            };
        }
    }
}