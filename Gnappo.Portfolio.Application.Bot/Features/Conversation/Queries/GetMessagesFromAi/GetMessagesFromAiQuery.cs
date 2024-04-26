using Gnappo.Portfolio.Application.Bot.Models;
using MediatR;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetMessagesFromAi
{
    public class GetMessagesFromAiQuery : IRequest<AiConversationResponse>
    {
    }
}
