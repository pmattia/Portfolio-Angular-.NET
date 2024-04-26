using Gnappo.Portfolio.Bot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Gnappo.Portfolio.Bot.Features.State.Queries.GetUserState;
using Gnappo.Portfolio.Application.Bot.Models;

namespace Gnappo.Portfolio.Bot.Dialogs
{
    public class GreetingDialog : BaseDialog
    {
        #region Variables
        private readonly IMediator _mediator;
        private readonly IList<CardAction> _suggestedIntents;
        private readonly string _mainFlowId;
        #endregion

        public GreetingDialog(string dialogId,
                              MessageFactoryWrapper messageFactory,
                              IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _mainFlowId = $"{nameof(GreetingDialog)}.mainFlow";

            _suggestedIntents = new List<CardAction>();
            _suggestedIntents.Add(new CardAction(
                    title: GetLocalizedString("IntentKowYou"),
                    text: GetLocalizedString("IntentKowYou"),
                    type: ActionTypes.MessageBack,
                    value: GetLocalizedString("IntentKowYou")
                ));
            _suggestedIntents.Add(new CardAction(
                    title: GetLocalizedString("IntentSkipPleasantries"),
                    text: GetLocalizedString("IntentSkipPleasantries"),
                    type: ActionTypes.MessageBack,
                    value: GetLocalizedString("IntentSkipPleasantries")
                ));
            _suggestedIntents.Add(new CardAction(
                    title: GetLocalizedString("IntentContactYou"),
                    text: GetLocalizedString("IntentContactYou"),
                    type: ActionTypes.MessageBack,
                    value: GetLocalizedString("IntentContactYou")
                ));

            var waterfallSteps = new WaterfallStep[]
            {
                    SuggestIntentAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }

        private async Task<DialogTurnResult> SuggestIntentAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var intent = await RecognizeIntentAsync(stepContext, cancellationToken);
            if (intent != Intent.Undefined) return await stepContext.EndDialogAsync(intent, cancellationToken);

            var userData = await _mediator.Send(new GetUserStateQuery() { Context = stepContext.Context });

            var nameCollectedBefore = (userData != null && userData.Name != null);
            var messageText = nameCollectedBefore ?
                    string.Format(GetLocalizedString("WelcomeBackAndAskIntent"), userData.Name) :
                    GetLocalizedString("GreetingAndAskIntent");

            var reply = MessageFactory.TextWithSuggestedActions(messageText, _suggestedIntents);

            await stepContext.Context.SendActivityAsync(reply);

            return new DialogTurnResult(DialogTurnStatus.Waiting, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var intent = await RecognizeIntentAsync(stepContext, cancellationToken);
            if (intent != Intent.Undefined)
            {
                return await stepContext.EndDialogAsync(intent, cancellationToken);
            }
            else
            {
                return await stepContext.EndDialogAsync(stepContext.Result, cancellationToken);
            }
        }
    }
}
