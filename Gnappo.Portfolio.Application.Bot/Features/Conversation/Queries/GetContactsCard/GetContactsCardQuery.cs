using MediatR;
using Microsoft.Bot.Schema;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetContactsCard
{
    public class GetContactsCardQuery : IRequest<Attachment>
    {
        public string CoverFileId { get; set; }
    }
}
