using Azure.AI.OpenAI.Assistants;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Models;
using Gnappo.Portfolio.Domain.Models;
using Gnappo.Portfolio.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Infrastructure.AI
{
    public class OpenAiService : ICognitiveService
    {
        private readonly OpenAiClient _client;
        private readonly ILogger<OpenAiService> _logger;

        public OpenAiService(IOptions<OpenAiSettings> settings, ILogger<OpenAiService> logger)
        {
            _logger = logger;
            _client = new OpenAiClient(settings.Value.ApiKey, settings.Value.AssistantId);
        }

        public async Task<AiResponse> GetLastResponseAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _client.GetLastMessageAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting last message from OpenAI");
                return new AiResponse()
                {
                    Success = false,
                    Text = e.Message
                };
            }
        }

        public async Task<string> SendMessageAsync(string message, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Sending message to OpenAI: {message}");
                return await _client.SendMessageAsync(message, cancellationToken);
            }
            catch (Exception e)
            {
                return e.Message;
            }   
        }
    }
}
