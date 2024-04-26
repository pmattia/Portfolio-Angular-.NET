using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Application.Bot.Resources;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Gnappo.Portfolio.Bot.Features.Conversation.Queries.RecognizeIntent
{
    public class RecognizeIntentQueryHandler: IRequestHandler<RecognizeIntentQuery, Intent>
    {
        private readonly IStringLocalizer _localizer;
        private readonly BotSettings _botSettings;

        public RecognizeIntentQueryHandler(IStringLocalizer<ConversationResources> localizer, IOptions<BotSettings> botSettings)
        {
            _localizer = localizer ?? throw new System.ArgumentNullException(nameof(localizer));
            _botSettings = botSettings.Value ?? throw new System.ArgumentNullException(nameof(botSettings));
        }

        public async Task<Intent> Handle(RecognizeIntentQuery request, CancellationToken cancellationToken)
        {
            var text = request.text;
            if (text == _localizer.GetString("ShowAllPosts").Value) return Intent.AboutBlog;

            if (text == _localizer.GetString("IntentKowYou").Value) return Intent.KnowYou;
            if (text == _localizer.GetString("IntentContactYou").Value || text == _localizer.GetString("IntentHowContactYou").Value) return Intent.ContactYou;
            if (text == _localizer.GetString("IntentGoAway").Value) return Intent.GoAway;
            if (text == _localizer.GetString("IntentSkipPleasantries").Value) return Intent.SkipPleasantries;   

            if (text == _localizer.GetString("ChangeTopic").Value) return Intent.TopicSuggestions;

            if (_botSettings.HasCognitiveService)
            {
                //todo: implement with cognitive service that recognizes intents
            }

            return Intent.Undefined;
        }
    }
}
