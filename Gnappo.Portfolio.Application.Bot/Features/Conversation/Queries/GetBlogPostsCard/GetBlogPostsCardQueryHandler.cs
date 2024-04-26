using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using MediatR;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using Gnappo.Portfolio.Application.Models;
using Microsoft.Extensions.Options;
using Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.AdaptiveCards;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetBlogPostsCard
{
    public class GetBlogPostsCardQueryHandler : IRequestHandler<GetBlogPostsCardQuery, Attachment>
    {
        private readonly IBlobService _blobService;
        private readonly IUrlFormatter _blogPostPageUrlFormatter;
        private readonly IUrlFormatter _storageUrlFormatter;

        public GetBlogPostsCardQueryHandler(IBlobService blobService, IOptions<WebClientSettings> webClientSettings)
        {
            _blogPostPageUrlFormatter = new ContentPageUrlFormatter(
                webClientSettings.Value.UrlPatterns.BlogPost,
                webClientSettings.Value.WebPortfolioUrl
                );

            _storageUrlFormatter = new StorageUrlFormatter(
                webClientSettings.Value.UrlPatterns.FileBinary
                );
            _blobService = blobService;
        }

        public async Task<Attachment> Handle(GetBlogPostsCardQuery request, CancellationToken cancellationToken)
        {
            var posts = await _blobService.GetBlogPostsAsync(cancellationToken);
            var actions = posts.Select(a => new AdaptiveCardAction()
            {
                name = a.Id,
                title = a.Title,
                type = "openUrl",
                contentType = PortfolioContentTypeEnum.Blog,
                url = _blogPostPageUrlFormatter.Format(a.Id)
            }).ToList();

            var coverUrl = string.Empty;
            if (!string.IsNullOrWhiteSpace(request.CoverFileId))
            {
                var image = _blobService.GetContentFile(request.CoverFileId);
                coverUrl = _storageUrlFormatter.Format(image.relativePath);
            }

            var additionalActions = request.AdditionalActions.Select(a => new AdaptiveCardAction()
            {
                title = a.Title,
                type = "messageBack",
                data = a.Text
            }).ToList();

            var adaptiveCard = new AdaptiveCardBuilder()
                        .SetCoverUrl(coverUrl)
                        .AddActions(actions)
                        .AddActions(additionalActions)
                        .Build();

            return new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCard, new JsonSerializerSettings { MaxDepth = null }),
            };
        }
    }
}