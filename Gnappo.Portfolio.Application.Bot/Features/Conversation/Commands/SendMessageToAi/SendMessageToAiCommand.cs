using Gnappo.Portfolio.Domain.Models;
using MediatR;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Commands.SendUserMessage
{
    public class SendMessageToAiCommand : IRequest<AiResponse>
    {
        public string TextMessage { get; set; }
    }
}
