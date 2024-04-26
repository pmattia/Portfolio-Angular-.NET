using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Bot.Helpers;
using MediatR;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Dialogs.Topics
{
    public class FailDialog : BaseDialog
    {
        #region Variables
        private readonly IMediator _mediator;
        private readonly string _mainFlowId;
        #endregion
        public FailDialog(string dialogId,
                             MessageFactoryWrapper messageFactory,
                             IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _mainFlowId = $"{nameof(FailDialog)}.mainFlow";

            var waterfallSteps = new WaterfallStep[]
            {
                    DontUnderstandStepAsync,
                    TopicsSuggestionStepAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }

        private async Task<DialogTurnResult> DontUnderstandStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptMessage = MessageFactory.DontUnderstand();
            await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);
                        
            await Task.Delay(mediumWait);
            promptMessage = MessageFactory.Text(GetLocalizedString("WeakAI"));
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
