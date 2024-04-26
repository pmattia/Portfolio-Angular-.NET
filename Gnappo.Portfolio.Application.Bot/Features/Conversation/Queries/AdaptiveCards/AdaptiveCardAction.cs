using Gnappo.Portfolio.Application.Contracts.Domain;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.AdaptiveCards
{
    public class AdaptiveCardAction
    {
        public string type { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public PortfolioContentTypeEnum contentType { get; set; }
        public string url { get; set; }
        public string data { get; set; }
    }
}