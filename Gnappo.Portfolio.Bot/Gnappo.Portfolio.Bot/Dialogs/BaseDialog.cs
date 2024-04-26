// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.18.1

using Gnappo.Portfolio.Application.Bot.Features.Conversation.Commands.SendUserMessage;
using Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetMessagesFromAi;
using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Bot.Features.Conversation.Queries.RecognizeIntent;
using Gnappo.Portfolio.Bot.Features.Localization.Queries.GetLocalString;
using Gnappo.Portfolio.Bot.Helpers;
using MediatR;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Dialogs
{
    public class BaseDialog : ComponentDialog
    {
        private const string HelpMsgText = "Show help here";
        private const string CancelMsgText = "Cancelling...";
        protected const int shortWait = 500;
        protected const int mediumWait = 1000;
        protected const int longWait = 3000;
        protected const int inactiveUserTimeout = 10000;
        private readonly MessageFactoryWrapper _messageFactory;
        private readonly IMediator _mediator;
        protected MessageFactoryWrapper MessageFactory
        {
            get
            {
                return _messageFactory;
            }
        }

        public BaseDialog(string dialogId, MessageFactoryWrapper messageFactory, IMediator mediator)
            : base(dialogId)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _messageFactory = messageFactory ?? throw new System.ArgumentNullException(nameof(messageFactory));
        }

        protected string GetLocalizedString(string key)
        {
            return _mediator.Send(new GetLocalStringQuery() { Key = key }).Result;
        }
        protected async Task<Intent> RecognizeIntentAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result == null) return Intent.Undefined;
            if (stepContext.Result is Intent) return (Intent) stepContext.Result;
            return await RecognizeIntentAsync(stepContext.Result.ToString(), cancellationToken);
        }
        protected async Task<Intent> RecognizeIntentAsync(string text, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new RecognizeIntentQuery() { text = text }, cancellationToken);
        }

        protected async Task<AiConversationResponse> TryConversateAsync(string message, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SendMessageToAiCommand() { TextMessage = message }, cancellationToken);
            return await _mediator.Send(new GetMessagesFromAiQuery(), cancellationToken);
            
        }

        protected override async Task<DialogTurnResult> OnBeginDialogAsync(DialogContext innerDc, object options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.OnBeginDialogAsync(innerDc, options, cancellationToken);
        }

        protected override async Task<DialogTurnResult> OnContinueDialogAsync(DialogContext innerDc, CancellationToken cancellationToken = default)
        {
            innerDc.Stack
                .ForEach(async dialog => Console.WriteLine(dialog.Id));   
            var result = await InterruptAsync(innerDc, cancellationToken);
            if (result != null)
            {
                return result;
            }

            return await base.OnContinueDialogAsync(innerDc, cancellationToken);
        }
        private async Task<DialogTurnResult> InterruptAsync(DialogContext innerDc, CancellationToken cancellationToken)
        {
            if (innerDc.Context.Activity.Type == ActivityTypes.Message)
            {
                var text = innerDc.Context.Activity.Text.ToLowerInvariant();

                switch (text)
                {
                    case "help":
                    case "?":
                        var helpMessage = MessageFactory.Text(HelpMsgText, AvatarEmotion.Smile);
                        await innerDc.Context.SendActivityAsync(helpMessage, cancellationToken);
                        return new DialogTurnResult(DialogTurnStatus.Waiting);

                    case "cancel":
                    case "quit":
                        var cancelMessage = MessageFactory.Text(CancelMsgText, AvatarEmotion.Smile);
                        await innerDc.Context.SendActivityAsync(cancelMessage, cancellationToken);
                        return await innerDc.CancelAllDialogsAsync(cancellationToken);
                }
            }

            return null;
        }
    }
}
