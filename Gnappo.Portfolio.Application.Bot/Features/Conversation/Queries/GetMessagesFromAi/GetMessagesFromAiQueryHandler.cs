using Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.AdaptiveCards;
using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using Gnappo.Portfolio.Application.Models;
using Gnappo.Portfolio.Domain.Models;
using MediatR;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetMessagesFromAi
{
    public class GetMessagesFromAiQueryHandler : IRequestHandler<GetMessagesFromAiQuery, AiConversationResponse>
    {
        private readonly BotSettings _botSettings;
        private readonly ICognitiveService _cognitiveService;
        private readonly IBlobService _blobService;
        private readonly Dictionary<PortfolioContentTypeEnum, IUrlFormatter> _contentPageUrlFormatter;

        public GetMessagesFromAiQueryHandler(IOptions<WebClientSettings> webClientSettings, IOptions<BotSettings> botSettings, ICognitiveService cognitiveService, IBlobService blobService)
        {
            _botSettings = botSettings.Value ?? throw new System.ArgumentNullException(nameof(botSettings));
            _cognitiveService = cognitiveService ?? throw new System.ArgumentNullException(nameof(cognitiveService));
            _blobService = blobService ?? throw new System.ArgumentNullException(nameof(blobService));

            _contentPageUrlFormatter = new Dictionary<PortfolioContentTypeEnum, IUrlFormatter>();
            _contentPageUrlFormatter.Add(PortfolioContentTypeEnum.Article, new ContentPageUrlFormatter(
                webClientSettings.Value.UrlPatterns.Article,
                webClientSettings.Value.WebPortfolioUrl
                ));

            _contentPageUrlFormatter.Add(PortfolioContentTypeEnum.Blog,new ContentPageUrlFormatter(
                webClientSettings.Value.UrlPatterns.BlogPost,
                webClientSettings.Value.WebPortfolioUrl
                ));
        }

        public async Task<AiConversationResponse> Handle(GetMessagesFromAiQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (_botSettings.HasCognitiveService)
                {
                    var aiResponse = await _cognitiveService.GetLastResponseAsync(cancellationToken);
                    if (aiResponse.Success)
                    {
                        var ret = new AiConversationResponse()
                        {
                            Success = aiResponse.Success,
                            Text = aiResponse.Text
                        };
                        var contentPages = new List<ContentPage>();
                        foreach (var item in aiResponse.DocumentIds)
                        {
                            var article = await _blobService.FindArticleByDocumentIdAsync(item, cancellationToken);
                            if (article != null)
                            {
                                contentPages.Add(article);
                            }
                            else
                            {
                                var blogPost = await _blobService.FindBlogPostsByDocumentIdAsync(item, cancellationToken);
                                if (blogPost != null)
                                {
                                    contentPages.Add(blogPost);
                                }   
                            }
                        }
                        //var testarticle = await _blobService.FindArticleByDocumentIdAsync("file-5qVJJTk2e9mHp35vmkyYJsq7", cancellationToken);
                        //if (testarticle != null)
                        //{
                        //    contentPages.Add(testarticle);
                        //}

                        ret.Suggestions = getSuggestionsCard(contentPages);
                        return ret;
                    }
                    else
                    {
                        return new AiConversationResponse()
                        {
                            Success = false,
                            Text = string.Empty
                        };
                    }
                }
                else
                {
                    return new AiConversationResponse()
                    {
                        Success = false,
                        Text = string.Empty
                    };
                }
            }
            catch (System.Exception e)
            {
                return new AiConversationResponse()
                {
                    Success = false,
                    Text = e.Message
                };
            }   
        }

        private Attachment getSuggestionsCard(IList<ContentPage> contents)
        {
            if(contents.Count() == 0)
            {
                return null;
            }   

            var actions = new List<AdaptiveCardAction>();
            foreach (var item in contents.ToList())
            {
                PortfolioContentTypeEnum? topicType = null;
                if (item is Article)
                {
                    topicType = PortfolioContentTypeEnum.Article;
                }
                else if (item is BlogPost)
                {
                    topicType = PortfolioContentTypeEnum.Blog;
                }
                if(topicType != null)
                {
                    actions.Add(new AdaptiveCardAction()
                    {
                        name = item.Id,
                        title = item.Title,
                        type = "openUrl",
                        contentType = topicType.Value,
                        url = _contentPageUrlFormatter[topicType.Value].Format(item.Id)
                    }); 
                }   
            }

            var adaptiveCard = new AdaptiveCardBuilder()
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
