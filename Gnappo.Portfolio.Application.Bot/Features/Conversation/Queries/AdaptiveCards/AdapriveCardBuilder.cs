using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.AdaptiveCards
{
    internal class AdaptiveCardBuilder
    {
        private string _coverUrl;
        private List<AdaptiveCardAction> _actions;

        public AdaptiveCardBuilder()
        {
            _actions = new List<AdaptiveCardAction>();
        }

        public AdaptiveCardBuilder SetCoverUrl(string bodyImageUrl)
        {
            _coverUrl = bodyImageUrl;
            return this;
        }
        public AdaptiveCardBuilder AddAction(AdaptiveCardAction action)
        {
            _actions.Add(action);
            return this;
        }
        public AdaptiveCardBuilder AddActions(IList<AdaptiveCardAction> actions)
        {
            _actions.AddRange(actions);
            return this;
        }

        public string Build()
        {
            var cardTemplate = string.Empty;
            if (string.IsNullOrWhiteSpace(_coverUrl))
            {
                cardTemplate = GetStringTemplate("adaptiveCardNoBodyTemplate.json");
            }
            else
            {
                cardTemplate = GetStringTemplate("adaptiveCardBodyImageTemplate.json");
                cardTemplate = cardTemplate.Replace("##BODYIMAGEURL##", _coverUrl);
            }

            var stringActions = new List<string>();
            foreach (var action in _actions)
            {
                //action types
                //     'openUrl', 'imBack', 'postBack','playAudio', 'playVideo', 'showImage', 'downloadFile',
                //     'signin', 'call', 'messageBack'.
                var actionTemplate = string.Empty;
                if (action.type == "openUrl")
                {
                    actionTemplate = GetStringTemplate("adaptiveCardOpenUrlAction.json");
                    actionTemplate = actionTemplate.Replace("##ACTIONTITLE##", action.title);
                    actionTemplate = actionTemplate.Replace("##ACTIONURL##", action.url);
                    actionTemplate = actionTemplate.Replace("##ACTIONCONTENTTYPE##", action.contentType.ToString());
                    actionTemplate = actionTemplate.Replace("##ACTIONCONTENTNAME##", action.name);
                }
                else
                {
                    actionTemplate = GetStringTemplate("adaptiveCardSubmitDataAction.json");
                    actionTemplate = actionTemplate.Replace("##ACTIONTITLE##", action.title);
                    actionTemplate = actionTemplate.Replace("##ACTIONDATA##", action.title);
                }
                stringActions.Add(actionTemplate);
            }

            cardTemplate = cardTemplate.Replace("##ACTIONS##", string.Join(',', stringActions));
            return cardTemplate;
        }

        private string GetStringTemplate(string filename)
        {
            var cardResourcePath = GetType().Assembly.GetManifestResourceNames().First(name => name.EndsWith(filename));
            using (var stream = GetType().Assembly.GetManifestResourceStream(cardResourcePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}