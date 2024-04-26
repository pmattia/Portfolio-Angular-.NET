
using Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetArticlesCard;
using Gnappo.Portfolio.Application.Features.Topics.Queries;
using Gnappo.Portfolio.Bot.Helpers;
using MediatR;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Dialogs.Contents
{
    public class ArticlesDialog : BaseDialog
    {
        #region Variables
        private readonly IMediator _mediator;
        private readonly string _mainFlowId;
        #endregion
        public ArticlesDialog(string dialogId,
                              MessageFactoryWrapper messageFactory,
                              IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _mainFlowId = $"{nameof(ArticlesDialog)}.mainFlow";

            var waterfallSteps = new WaterfallStep[]
            {
                    SuggestArticlesAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }


        private async Task<DialogTurnResult> SuggestArticlesAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var topic = (TopicInfoDto)stepContext.Options ?? new TopicInfoDto();
            var additionalActions = new List<CardAction>() { MessageFactory.ChangeTopicAction() };
            var articlesAdaptiveCard = _mediator.Send(new GetArticlesCardQuery()
            {
                Tags = topic.Tags,
                CoverFileId = topic.CoverFileId,
                AdditionalActions = additionalActions,
                PdfFileIds = topic.PdfFileIds
            }
            ).Result;
            var response = MessageFactory.Attachment(articlesAdaptiveCard);

            await stepContext.Context.SendActivityAsync(response, cancellationToken);

            return new DialogTurnResult(DialogTurnStatus.Waiting, cancellationToken);
        }


        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(stepContext.Result, cancellationToken);
        }

    }
}
