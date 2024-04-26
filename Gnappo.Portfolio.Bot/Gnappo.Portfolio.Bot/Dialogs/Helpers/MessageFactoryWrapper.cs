using Gnappo.Portfolio.Application.Bot.Resources;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Application.Features.Avatars.Queries.GetAvatarChannelData;
using Gnappo.Portfolio.Application.Features.Storage.Queries;
using Gnappo.Portfolio.Bot.Dialogs.Helpers;
using Gnappo.Portfolio.Bot.Features.Localization.Queries.GetLocalString;
using MediatR;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Gnappo.Portfolio.Bot.Helpers
{
    public class MessageFactoryWrapper
    {
        #region variables
        private readonly IMediator _mediator;
        #endregion

        public MessageFactoryWrapper(IMediator mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public Activity Text(
            string text,
            AvatarEmotion avatarEmotion = AvatarEmotion.Smile,
            BotStatus botStatus = BotStatus.Waiting,
            string inputHint = null
            )
        {
            var message = MessageFactory.Text(text, text, inputHint);
            var avatarData =  _mediator.Send(new GetAvatarDataQuery() { Emotion = avatarEmotion }).Result;
            message.ChannelData = AddTypingStatus(avatarData, botStatus);

            return message;
        }

        public Activity DontUnderstand(
            BotStatus botStatus = BotStatus.Waiting,
            string inputHint = null)
        {
            var text = _mediator.Send(new GetLocalStringQuery() { Key = "DontUnderstand" }).Result;
            var message = MessageFactory.Text(text, text, inputHint);
            var avatarData = _mediator.Send(new GetAvatarDataQuery() { Emotion = AvatarEmotion.Embarassed }).Result;
            message.ChannelData = AddTypingStatus(avatarData, botStatus);

            return message;
        }

        public Activity TextWithSuggestedActions(
            string text, IList<CardAction> cardActions, 
            AvatarEmotion avatarEmotion = AvatarEmotion.Smile,
            BotStatus botStatus = BotStatus.Waiting, 
            string inputHint = null)
        {
            var message = MessageFactory.Text(text, text, inputHint);
            var avatarData = _mediator.Send(new GetAvatarDataQuery() { Emotion = avatarEmotion }).Result;
            message.ChannelData = AddTypingStatus(avatarData, botStatus);
            if (cardActions != null)
            {
                message.SuggestedActions = new SuggestedActions()
                {
                    Actions = cardActions,
                };
            }
            return message;
        }

        public Activity Attachment(
            Attachment attachment, 
            string text = null, 
            AvatarEmotion avatarEmotion = AvatarEmotion.Smile,
            BotStatus botStatus = BotStatus.Waiting, 
            string inputHint = null)
        {
            var message = MessageFactory.Attachment(attachment, text, text, inputHint);
            var avatarData = _mediator.Send(new GetAvatarDataQuery() { Emotion = avatarEmotion }).Result;
            message.ChannelData = AddTypingStatus(avatarData, botStatus);

            return (Activity)message;
        }


        public Activity ContentUrl(
            ContentUrlDto contantUrl, 
            AvatarEmotion avatarEmotion = AvatarEmotion.Smile, 
            BotStatus botStatus = BotStatus.Waiting, 
            string inputHint = null)
        {
            var message = MessageFactory.ContentUrl(contantUrl.Url, contantUrl.ContentType, null, null, null, inputHint);
            var avatarData = _mediator.Send(new GetAvatarDataQuery() { Emotion = avatarEmotion }).Result;
            message.ChannelData = AddTypingStatus(avatarData, botStatus);

            return (Activity)message;
        }

        private Dictionary<string,string> AddTypingStatus(Dictionary<string, string> channelData, BotStatus botStatus)
        {
            if (channelData.ContainsKey("isTyping"))
            {
                channelData["isTyping"] = (BotStatus.Typing == botStatus).ToString();
            }
            else
            {
                channelData.Add("isTyping", (BotStatus.Typing == botStatus).ToString());
            }
            return channelData;
        }

        public CardAction ChangeTopicAction()
        {
            return new CardAction(
                    title: _mediator.Send(new GetLocalStringQuery() { Key = "ChangeTopic" }).Result,
                    text: _mediator.Send(new GetLocalStringQuery() { Key = "ChangeTopic" }).Result,
                    type: ActionTypes.MessageBack
                );
        }
    }
}
