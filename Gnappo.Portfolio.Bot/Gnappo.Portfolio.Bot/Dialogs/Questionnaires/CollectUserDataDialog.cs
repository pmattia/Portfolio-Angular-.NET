using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Bot.Features.State.Commands.UpdateUserState;
using Gnappo.Portfolio.Bot.Features.State.Queries.GetUserState;
using Gnappo.Portfolio.Bot.Helpers;
using MediatR;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Dialogs.Questionnaires
{
    public class CollectUserDataDialog : BaseDialog
    {
        #region Variables
        private readonly IList<CardAction> _reasonSuggestions;
        private readonly IMediator _mediator;
        private readonly string _nameDialogId;
        private readonly string _reasonDialogId;
        private readonly string _emailDialogId;
        private readonly string _mainFlowId;
        #endregion
        public CollectUserDataDialog(string dialogId, MessageFactoryWrapper messageFactory, IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));

            _mainFlowId = $"{nameof(CollectUserDataDialog)}.mainFlow";
            _nameDialogId = $"{nameof(CollectUserDataDialog)}.name";
            _reasonDialogId = $"{nameof(CollectUserDataDialog)}.reason";
            _emailDialogId = $"{nameof(CollectUserDataDialog)}.email";

            _reasonSuggestions = new List<CardAction>();
            _reasonSuggestions.Add(
                new CardAction()
                {
                    Title = GetLocalizedString("UserReason1"),
                    Text = GetLocalizedString("UserReason1"),
                    Type = ActionTypes.MessageBack
                }
                );
            _reasonSuggestions.Add(
                new CardAction()
                {
                    Title = GetLocalizedString("UserReason2"),
                    Text = GetLocalizedString("UserReason2"),
                    Type = ActionTypes.MessageBack
                }
                );
            _reasonSuggestions.Add(
                new CardAction()
                {
                    Title = GetLocalizedString("UserReason3"),
                    Text = GetLocalizedString("UserReason3"),
                    Type = ActionTypes.MessageBack
                }
                );
            _reasonSuggestions.Add(
                new CardAction()
                {
                    Title = GetLocalizedString("NoComment"),
                    Text = GetLocalizedString("NoComment"),
                    Type = ActionTypes.MessageBack
                }
                );

            AddDialog(new TextPrompt(_nameDialogId));
            AddDialog(new TextPrompt(_reasonDialogId));
            AddDialog(new TextPrompt(_emailDialogId));

            var waterfallSteps = new WaterfallStep[]
            {
                    AskNameStepAsync,
                    AskReasonStepAsync,
                    AskEmailStepAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }

        private async Task<DialogTurnResult> AskNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userData = await _mediator.Send(new GetUserStateQuery() { Context = stepContext.Context });

            if (userData.Name == null)
            {
                var promptMessage = MessageFactory.Text(GetLocalizedString("FriendlyAskName"), AvatarEmotion.Doubt);
                return await stepContext.PromptAsync(_nameDialogId, new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> AskReasonStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var intent = await RecognizeIntentAsync(stepContext, cancellationToken);
            if (intent != Intent.Undefined) return await stepContext.EndDialogAsync(intent, cancellationToken);

            var userData = await _mediator.Send(new GetUserStateQuery() { Context = stepContext.Context });

            if (stepContext.Result != null)
            {
                userData.Name = stepContext.Result.ToString();
                await _mediator.Send(new UpdateUserStateCommand() { Context = stepContext.Context, UserData = userData });

                var promptMessage = MessageFactory.Text(string.Format(GetLocalizedString("NiceToMeetYou"), userData.Name));
                await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);
            }

            if (userData.Reason == null)
            {
                var promptMessage = MessageFactory.TextWithSuggestedActions(GetLocalizedString("AskReason"), _reasonSuggestions);
                return await stepContext.PromptAsync(_reasonDialogId, new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> AskEmailStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var intent = await RecognizeIntentAsync(stepContext, cancellationToken);
            if (intent != Intent.Undefined) return await stepContext.EndDialogAsync(intent, cancellationToken);

            var userData = await _mediator.Send(new GetUserStateQuery() { Context = stepContext.Context });

            if (stepContext.Result != null)
            {
                userData.Reason = stepContext.Result.ToString(); ;
                await _mediator.Send(new UpdateUserStateCommand() { Context = stepContext.Context, UserData = userData });
            }

            if (userData.Contact == null)
            {
                var promptMessage = MessageFactory.Text(GetLocalizedString("AskContact"), AvatarEmotion.Doubt);
                return await stepContext.PromptAsync(_emailDialogId, new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result != null)
            {
                var userData = await _mediator.Send(new GetUserStateQuery() { Context = stepContext.Context });

                userData.Contact = stepContext.Result.ToString();
                await _mediator.Send(new UpdateUserStateCommand() { Context = stepContext.Context, UserData = userData });

            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
