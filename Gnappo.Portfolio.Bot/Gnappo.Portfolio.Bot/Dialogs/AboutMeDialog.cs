using Gnappo.Portfolio.Bot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using Gnappo.Portfolio.Application.Contracts.Domain;
using MediatR;
using Gnappo.Portfolio.Application.Features.Storage.Queries.GetContentUrl;
using Gnappo.Portfolio.Bot.Dialogs.Helpers;
using Gnappo.Portfolio.Application.Bot.Models;

namespace Gnappo.Portfolio.Bot.Dialogs.Topics
{
    public class AboutMeDialog : BaseDialog
    {
        #region Variables
        private readonly IMediator _mediator;
        private readonly string _mainFlowId;
        #endregion
        public AboutMeDialog(string dialogId,
                             MessageFactoryWrapper messageFactory,
                             IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _mainFlowId = $"{nameof(AboutMeDialog)}.mainFlow";

            var waterfallSteps = new WaterfallStep[]
            {
                    MonologueStepAsync,
                    TopicsSuggestionStepAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }

        private async Task<DialogTurnResult> MonologueStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var getContentUrlQuery = new GetContentUrlQuery() { contentName = "me-pic" };
            var contentUrl = await _mediator.Send(getContentUrlQuery);

            if (contentUrl != null)
            {
                await Task.Delay(shortWait);
                var thisIsMeMessage = MessageFactory.Text(
                    GetLocalizedString("ThisIsMe"),
                    AvatarEmotion.Lol,
                    BotStatus.Typing
                    );
                await stepContext.Context.SendActivityAsync(thisIsMeMessage, cancellationToken);

                await Task.Delay(shortWait);
                var img = MessageFactory.ContentUrl(
                    contentUrl,
                    AvatarEmotion.Lol,
                    BotStatus.Typing
                    );
                await stepContext.Context.SendActivityAsync(img, cancellationToken);
            }

            await Task.Delay(mediumWait);
            var promptMessage = MessageFactory.Text(
                GetLocalizedString("WhoIAm"),
                AvatarEmotion.Talk,
                BotStatus.Typing);
            await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);

            await Task.Delay(mediumWait);
            promptMessage = MessageFactory.Text(
                GetLocalizedString("WhatILike"),
                AvatarEmotion.Talk,
                BotStatus.Typing
                );
            await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);

            await Task.Delay(shortWait);
            promptMessage = MessageFactory.Text(
                GetLocalizedString("WhatIDo"),
                AvatarEmotion.Talk,
                BotStatus.Typing
                );
            await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> TopicsSuggestionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await Task.Delay(longWait);
            return await stepContext.NextAsync(Intent.TopicSuggestions, cancellationToken);
        }


        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(stepContext.Result, cancellationToken);
        }
    }
}
