using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Options;
using Gnappo.Portfolio.Domain.Models;
using Gnappo.Portfolio.Application.Bot.Models;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Commands.SendUserMessage
{
    public class SendMessageToAiCommandHandler : IRequestHandler<SendMessageToAiCommand, AiResponse>
    {
        private readonly BotSettings _botSettings;
        private readonly ICognitiveService _cognitiveService;


        public SendMessageToAiCommandHandler(IOptions<BotSettings> botSettings, ICognitiveService cognitiveService)
        {
            _botSettings = botSettings.Value ?? throw new System.ArgumentNullException(nameof(botSettings));
            _cognitiveService = cognitiveService ?? throw new System.ArgumentNullException(nameof(cognitiveService));
        }

        /// <summary>
        /// return user message id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AiResponse> Handle(SendMessageToAiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_botSettings.HasCognitiveService)
                {
                    var response = await _cognitiveService.SendMessageAsync(request.TextMessage, cancellationToken);
                    return new AiResponse()
                    {
                        Success = true,
                        Text = response
                    };
                }
                else
                {
                    return new AiResponse()
                    {
                        Success = false,
                        Text = string.Empty
                    };
                }
            }
            catch (System.Exception e)
            {
                return new AiResponse()
                {
                    Success = false,
                    Text = e.Message
                };
            }
        }
    }
}