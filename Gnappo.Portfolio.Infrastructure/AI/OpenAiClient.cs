using Azure;
using Azure.AI.OpenAI.Assistants;
using Gnappo.Portfolio.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Infrastructure.AI
{
    internal class OpenAiClient
    {
        private readonly string _apiKey;
        private readonly string _assistantId;
        private readonly ExtractOpenAiDocumentIds _documentIdExtractor;

        private Assistant _assistant;
        private Assistant Assistant
        {
            get
            {
                return _assistant ??= _client.GetAssistant(_assistantId).Value;
            }
        }

        private AssistantsClient _client;
        private AssistantsClient Client
        {
            get
            {
                return _client ??= new AssistantsClient(_apiKey);
            }
        }

        private AssistantThread _thread; 
        private AssistantThread Thread
        {
            get
            {
                return _thread ??= Client.CreateThread();
            }
        }

        public OpenAiClient(string apiKey, string assistantId)
        {
            _apiKey = apiKey;
            _assistantId = assistantId;
            _documentIdExtractor = new ExtractOpenAiDocumentIds();
        }

        public async Task<string> SendMessageAsync(string textMessage, CancellationToken cancellationToken)
        {
            Response<ThreadMessage> messageResponse = await Client.CreateMessageAsync(
                    Thread.Id,
                    MessageRole.User,
                    textMessage,
                    null,
                    null,
                    cancellationToken);
            ThreadMessage message = messageResponse.Value;
            return message.Id;
        }

        public async Task<AiResponse> GetLastMessageAsync(CancellationToken cancellationToken)
        {
            Response<ThreadRun> runResponse = await Client.CreateRunAsync(Thread.Id,
                                                                          new CreateRunOptions(Assistant.Id),
                                                                          cancellationToken
                                                                          );
            ThreadRun run = runResponse.Value;

            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                runResponse = await Client.GetRunAsync(Thread.Id, runResponse.Value.Id);
            }
            while (runResponse.Value.Status == RunStatus.Queued || runResponse.Value.Status == RunStatus.InProgress);

            if (runResponse.Value.Status == RunStatus.Failed)
            {
                throw new Exception("I'm sorry, I'm not able to answer at the moment");
            }

            Response<PageableList<ThreadMessage>> retrievalResponse = await Client.GetMessagesAsync(Thread.Id,null,null,null,null, cancellationToken);
            IReadOnlyList<ThreadMessage> messages = retrievalResponse.Value.Data;

            ThreadMessage retrievedMessage = messages.Where(m => m.Role == MessageRole.Assistant).FirstOrDefault();

            var response = new AiResponse();
            foreach (MessageContent contentItem in retrievedMessage.ContentItems)
            {
                if (contentItem is MessageTextContent textItem)
                {
                    //extract references to files
                    var assistantFiles = _documentIdExtractor.ExtractDocumentIds(textItem);
                    var strippedText = textItem.Text;
                    foreach (var file in assistantFiles)
                    {
                        strippedText = strippedText.Replace(file.Key, string.Empty);
                    }
                    response.Text += strippedText;
                    response.DocumentIds = assistantFiles.Select(f => f.Value).ToList();
                }
                else if (contentItem is MessageImageFileContent imageFileItem)
                {
                    throw new Exception("I'm sorry, I'm not able to answer at the moment");
                }
            }

            response.Success = true;
            return response;
        }   
    }
}
