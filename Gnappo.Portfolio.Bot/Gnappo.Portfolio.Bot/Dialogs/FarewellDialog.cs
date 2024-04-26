using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Application.Features.Storage.Queries.GetContentUrl;
using Gnappo.Portfolio.Bot.Helpers;
using MediatR;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Dialogs
{
    public class FarewellDialog : BaseDialog
    {
        #region Variables
        private readonly IMediator _mediator;
        private readonly string _mainFlowId;
        #endregion
        public FarewellDialog(string dialogId,
                             MessageFactoryWrapper messageFactory,
                             IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            
            var waterfallSteps = new WaterfallStep[]
            {
                    FarewellAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }

        private async Task<DialogTurnResult> FarewellAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var getContentUrlQuery = new GetContentUrlQuery() { contentName = "farewell" };
            var contentUrl = await _mediator.Send(getContentUrlQuery);

            if (contentUrl != null)
            {
                var thisIsMeMessage = MessageFactory.Text(GetLocalizedString("Farewell"), AvatarEmotion.Doubt);
                await stepContext.Context.SendActivityAsync(thisIsMeMessage, cancellationToken);

                await Task.Delay(shortWait);
                var img = MessageFactory.ContentUrl(
                    contentUrl,
                    AvatarEmotion.Doubt
                    );
                await stepContext.Context.SendActivityAsync(img, cancellationToken);
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(Intent.GoAway, cancellationToken);
        }
    }
}
