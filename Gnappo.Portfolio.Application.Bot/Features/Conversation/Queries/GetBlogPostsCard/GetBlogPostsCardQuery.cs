using MediatR;
using Microsoft.Bot.Schema;
using System.Collections.Generic;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetBlogPostsCard
{
    public class GetBlogPostsCardQuery : IRequest<Attachment>
    {
        public string CoverFileId { get; set; }
        public IEnumerable<CardAction> AdditionalActions { get; set; }
    }
}
