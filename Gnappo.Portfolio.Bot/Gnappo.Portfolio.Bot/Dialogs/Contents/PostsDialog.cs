using Gnappo.Portfolio.Bot.Helpers;
using MediatR;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gnappo.Portfolio.Application.Features.Topics.Queries;
using Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetBlogPostsCard;

namespace Gnappo.Portfolio.Bot.Dialogs.Contents
{
    public class PostsDialog : BaseDialog
    {
        #region Variables
        private readonly string _mainFlowId;
        private readonly IMediator _mediator;
        #endregion
        public PostsDialog(string dialogId,
                           MessageFactoryWrapper messageFactory,
                           IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _mainFlowId = $"{nameof(PostsDialog)}.mainFlow";

            var waterfallSteps = new WaterfallStep[]
            {
                    SuggestPostsAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }


        private async Task<DialogTurnResult> SuggestPostsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var topic = (TopicInfoDto)stepContext.Options ?? new TopicInfoDto();
            var additionalActions = new List<CardAction>() { MessageFactory.ChangeTopicAction() };
            var postsAdaptiveCard = _mediator.Send(new GetBlogPostsCardQuery()
            {
                CoverFileId= topic.CoverFileId,
                AdditionalActions = additionalActions
            }
            ).Result;
            var response = MessageFactory.Attachment(postsAdaptiveCard);

            await stepContext.Context.SendActivityAsync(response, cancellationToken);

            return new DialogTurnResult(DialogTurnStatus.Waiting, cancellationToken);
        }


        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(stepContext.Result, cancellationToken);
        }

    }
}
