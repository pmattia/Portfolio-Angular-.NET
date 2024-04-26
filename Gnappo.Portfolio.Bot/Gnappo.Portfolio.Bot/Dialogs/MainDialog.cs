// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.18.1

using Azure;
using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Application.Bot.Resources;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Bot.Dialogs.Helpers;
using Gnappo.Portfolio.Bot.Dialogs.Questionnaires;
using Gnappo.Portfolio.Bot.Dialogs.Suggestions;
using Gnappo.Portfolio.Bot.Dialogs.Topics;
using Gnappo.Portfolio.Bot.Features.State.Queries.GetUserState;
using Gnappo.Portfolio.Bot.Helpers;
using MediatR;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Dialogs
{
    public class MainDialog : BaseDialog
    {
        #region Variables
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ICognitiveService _cognitiveService;
        private readonly string _greetingDialogId;
        private readonly string _aboutMeDialogId;
        private readonly string _collectDataDialogId;
        private readonly string _topicDialogId;
        private readonly string _contactMeDialogId;
        private readonly string _farewellDialogId;
        private readonly string _failDialogId;
        private readonly string _mainFlowId;
        #endregion  

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(ILogger<MainDialog> logger,
                          IMediator mediator,
                          ICognitiveService cognitiveService)
            : base(nameof(MainDialog), new MessageFactoryWrapper(mediator), mediator)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _cognitiveService = cognitiveService ?? throw new System.ArgumentNullException(nameof(cognitiveService));   

            _greetingDialogId = $"{nameof(MainDialog)}.greeting";
            _aboutMeDialogId = $"{nameof(MainDialog)}.aboutMe";
            _collectDataDialogId = $"{nameof(MainDialog)}.collectUserData";
            _topicDialogId = $"{nameof(MainDialog)}.topicsSuggestion";
            _contactMeDialogId = $"{nameof(MainDialog)}.contact";
            _farewellDialogId = $"{nameof(MainDialog)}.farewell";
            _failDialogId = $"{nameof(MainDialog)}.fail";
            _mainFlowId = $"{nameof(MainDialog)}.mainFlow";

            AddDialog(new GreetingDialog(_greetingDialogId, MessageFactory, _mediator));
            AddDialog(new AboutMeDialog(_aboutMeDialogId, MessageFactory, _mediator));
            AddDialog(new CollectUserDataDialog(_collectDataDialogId, MessageFactory, _mediator));
            AddDialog(new TopicsSuggestionDialog(_topicDialogId, MessageFactory, _mediator));
            AddDialog(new FarewellDialog(_farewellDialogId, MessageFactory, _mediator));
            AddDialog(new FailDialog(_failDialogId, MessageFactory, _mediator)); 
            AddDialog(new ContactMeDialog(_contactMeDialogId, MessageFactory, _mediator));

            var waterfallSteps = new WaterfallStep[]
            {
                    IntroStepAsync,
                    ActStepAsync,
                //    CollectContactsStepAsync,
                    SuggestionStepAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Options == null) //no text coming from previous dialog
            {
                _logger.LogInformation("MainDialog: IntroStepAsync: no options");
                return await stepContext.BeginDialogAsync(_greetingDialogId, null, cancellationToken);                
            }
            else
            {
                if (stepContext.Options is Intent)
                {
                    return await stepContext.NextAsync(stepContext.Options, cancellationToken);
                }
                else
                {
                    var intent = await RecognizeIntentAsync(stepContext.Options.ToString(), cancellationToken);
                    if (intent != Intent.Undefined) return await stepContext.NextAsync(intent, cancellationToken);

                    return await stepContext.NextAsync(stepContext.Options, cancellationToken);
                }
            }
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is Intent)
            {
                switch ((Intent)stepContext.Result)
                {
                    case Intent.KnowYou:
                        return await stepContext.BeginDialogAsync(_aboutMeDialogId, null, cancellationToken);
                        break;
                    case Intent.ContactYou:
                        return await stepContext.BeginDialogAsync(_contactMeDialogId, null, cancellationToken);
                        break;
                    case Intent.GoAway:
                        return await stepContext.BeginDialogAsync(_farewellDialogId, null, cancellationToken);
                        break;
                    case Intent.SkipPleasantries:
                        return await stepContext.BeginDialogAsync(_topicDialogId, GetLocalizedString("SuggestStuff"), cancellationToken);
                        break;
                    case Intent.TopicSuggestions:
                        return await stepContext.BeginDialogAsync(_topicDialogId, GetLocalizedString("SuggestOtherTopics"), cancellationToken);
                        break;
                    case Intent.Undefined:
                    default:
                        return await stepContext.BeginDialogAsync(_failDialogId, null, cancellationToken);
                        break;
                }
            }
            else
            {
                var response = await TryConversateAsync(stepContext.Result.ToString(), cancellationToken);
                if (response.Success)
                {
                    if(response.Suggestions != null)
                    {

                        var message = MessageFactory.Text(
                            response.Text, 
                            AvatarEmotion.Smile,
                            BotStatus.Typing
                            );
                        await stepContext.Context.SendActivityAsync(message, cancellationToken);

                        var suggestions = MessageFactory.Attachment(response.Suggestions);
                        await stepContext.Context.SendActivityAsync(suggestions, cancellationToken);
                        return await stepContext.NextAsync(null, cancellationToken);
                    }
                    else
                    {
                        var message = MessageFactory.Text(
                            response.Text, 
                            AvatarEmotion.Smile,
                            BotStatus.Typing
                            );
                        await stepContext.Context.SendActivityAsync(message, cancellationToken);
                        return await stepContext.NextAsync(null, cancellationToken);
                    }
                }
                else
                {
                    return await stepContext.BeginDialogAsync(_failDialogId, null, cancellationToken);
                }
            }
        }

        /// <summary>
        /// NOT REFERENCED
        /// </summary>
        /// <param name="stepContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<DialogTurnResult> CollectContactsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result == null)
            {
                var userData = await _mediator.Send(new GetUserStateQuery() { Context = stepContext.Context });

                await Task.Delay(longWait);
                if (userData.Name == null)
                {
                    //todo: use cognitive?
                }

                return await stepContext.BeginDialogAsync(_collectDataDialogId, userData, cancellationToken);
            }
            else
            {
                return await stepContext.NextAsync(stepContext.Result, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> SuggestionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result == null)
            {
                await Task.Delay(mediumWait);
                return await stepContext.BeginDialogAsync(_topicDialogId, GetLocalizedString("SuggestOtherTopics"), cancellationToken);
            }
            else
            {
                return await stepContext.NextAsync(stepContext.Result, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result == null)
            {
                return await stepContext.ReplaceDialogAsync(InitialDialogId, Intent.TopicSuggestions, cancellationToken);
            }
            else if(stepContext.Result is Intent)
            {
                if ((Intent)stepContext.Result == Intent.GoAway)
                {
                    _logger.LogInformation("MainDialog: FinalStepAsync: GoAway");
                    return await stepContext.EndDialogAsync(null, cancellationToken);
                }
                else
                {
                    return await stepContext.ReplaceDialogAsync(InitialDialogId, stepContext.Result, cancellationToken);
                }
            }
            else
            {
                return await stepContext.ReplaceDialogAsync(InitialDialogId, stepContext.Result, cancellationToken);
            }
        }
    }
}
