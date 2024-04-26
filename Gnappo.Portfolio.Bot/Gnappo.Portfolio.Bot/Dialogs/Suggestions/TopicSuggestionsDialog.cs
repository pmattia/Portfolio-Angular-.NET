using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Application.Features.Articles.Queries.GetMainTopics;
using Gnappo.Portfolio.Bot.Dialogs.Contents;
using Gnappo.Portfolio.Bot.Helpers;
using MediatR;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Dialogs.Suggestions
{
    public class TopicsSuggestionDialog : BaseDialog
    {
        #region Variables
        private IList<CardAction> _mainTopics;
        private readonly IMediator _mediator;
        private readonly string _articlesDialogId;
        private readonly string _postsDialogId;
        private readonly string _memesDialogId;
        private readonly string _mainFlowId;
        #endregion
        public TopicsSuggestionDialog(string dialogId,
                                      MessageFactoryWrapper messageFactory,
                                      IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));

            _articlesDialogId = $"{nameof(TopicsSuggestionDialog)}.articles";
            _postsDialogId = $"{nameof(TopicsSuggestionDialog)}.posts";
            _memesDialogId = $"{nameof(TopicsSuggestionDialog)}.memes";
            _mainFlowId = $"{nameof(TopicsSuggestionDialog)}.mainFlow";

            _mainTopics = new List<CardAction>();

            var waterfallSteps = new WaterfallStep[]
            {
                    TopicsSuggestionStepAsync,
                    SelectedTopicAsync,
                    FinalStepAsync,
            };

            AddDialog(new ArticlesDialog(_articlesDialogId, messageFactory, _mediator));
            AddDialog(new PostsDialog(_postsDialogId, messageFactory, _mediator));
            AddDialog(new FunnyStuffDialog(_memesDialogId, messageFactory, _mediator));

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }


        private async Task<DialogTurnResult> TopicsSuggestionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var message = stepContext.Options != null ? stepContext.Options.ToString() : GetLocalizedString("SuggestStuff");
            var actions = new List<CardAction>();

            //always update the main topics
            _mainTopics = _mediator.Send(new GetMainTopicsQuery()).Result
                            .Select(topic => new CardAction(
                                                title: topic.Title,
                                                text: topic.Title,
                                                type: ActionTypes.MessageBack,
                                                value: topic
                                                )
                            ).ToList();

            actions.AddRange(_mainTopics);
            actions.Add(new CardAction(
                    title: GetLocalizedString("IntentHowContactYou"),
                    text: GetLocalizedString("IntentHowContactYou"),
                    type: ActionTypes.MessageBack,
                    value: GetLocalizedString("IntentHowContactYou")
                ));

            var prompt = MessageFactory.TextWithSuggestedActions(message, actions);
            await stepContext.Context.SendActivityAsync(prompt, cancellationToken);
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        private async Task<DialogTurnResult> SelectedTopicAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (IsTopic(stepContext.Result))
            {
                var selectedTopic = stepContext.Result.ToString();
                var topic = await _mediator.Send(new GetTopicQuery() { Name = selectedTopic });

                switch (topic.Type)
                {
                    case PortfolioContentTypeEnum.Article:
                        return await stepContext.BeginDialogAsync(_articlesDialogId, topic, cancellationToken);
                        break;
                    case PortfolioContentTypeEnum.Blog:
                        return await stepContext.BeginDialogAsync(_postsDialogId, topic, cancellationToken);
                        break;
                    case PortfolioContentTypeEnum.Funny:
                        return await stepContext.BeginDialogAsync(_memesDialogId, null, cancellationToken);
                        break;
                    case PortfolioContentTypeEnum.Contact:
                        return await stepContext.NextAsync(Intent.ContactYou, cancellationToken);
                        break;
                    case PortfolioContentTypeEnum.ContentUrl:
                    default:
                        return await stepContext.NextAsync(Intent.Undefined, cancellationToken);
                }
            }
            return await stepContext.NextAsync(stepContext.Result, cancellationToken);
        }

        private bool IsTopic(object title)
        {
            return title != null && _mainTopics.Select(t => t.Title).Contains(title.ToString());
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result != null)
            {
                var intent = await RecognizeIntentAsync(stepContext, cancellationToken);
                if (intent != Intent.Undefined)
                {
                    return await stepContext.EndDialogAsync(intent, cancellationToken);
                }
            }
            return await stepContext.EndDialogAsync(stepContext.Result, cancellationToken);
        }

    }
}
