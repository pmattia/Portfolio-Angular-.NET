using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Application.Features.Storage.Queries.GetMemes;
using Gnappo.Portfolio.Bot.Helpers;
using MediatR;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Dialogs.Contents
{
    public class FunnyStuffDialog : BaseDialog
    {
        #region Variables
        private readonly IMediator _mediator;
        private readonly string _mainFlowId;
        #endregion
        public FunnyStuffDialog(string dialogId,
                              MessageFactoryWrapper messageFactory,
                              IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _mainFlowId = $"{nameof(FunnyStuffDialog)}.mainFlow";

            var waterfallSteps = new WaterfallStep[]
            {
                    SuggestMemesAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }


        private async Task<DialogTurnResult> SuggestMemesAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var memes = _mediator.Send(new GetMemesQuery() { }).Result;
            foreach(var meme in memes)
            {
                var attachment = MessageFactory.ContentUrl(
                    meme,
                    AvatarEmotion.Lol
                    );

                await stepContext.Context.SendActivityAsync(attachment, cancellationToken);
                await Task.Delay(mediumWait);
            }


            return await stepContext.NextAsync(null, cancellationToken);
        }


        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

    }
}
