using MediatR;
using Microsoft.Bot.Schema;
using System.Collections.Generic;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetArticlesCard
{
    public class GetArticlesCardQuery : IRequest<Attachment>
    {
        public IEnumerable<string> Tags { get; set; }
        public string CoverFileId { get; set; }
        public IEnumerable<CardAction> AdditionalActions { get; set; }
        public IEnumerable<string> PdfFileIds { get; set; }
    }
}
