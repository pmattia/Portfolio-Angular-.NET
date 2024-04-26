using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Avatars.Queries.GetAvatarChannelData;
using Gnappo.Portfolio.Bot.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Mime;
using Gnappo.Portfolio.Bot.Features.State.Queries.GetUserState;
using Gnappo.Portfolio.Bot.Dialogs.Suggestions;
using Gnappo.Portfolio.Application.Contracts.Domain;
using MediatR;
using Gnappo.Portfolio.Application.Features.Storage.Queries.GetContentUrl;
using Gnappo.Portfolio.Application.Bot.Features.Conversation.Queries.GetContactsCard;

namespace Gnappo.Portfolio.Bot.Dialogs.Topics
{
    public class ContactMeDialog : BaseDialog
    {
        #region Variables
        private readonly IMediator _mediator;
        private readonly string _mainFlowId;
        #endregion
        public ContactMeDialog(string dialogId,
                             MessageFactoryWrapper messageFactory,
                             IMediator mediator) : base(dialogId, messageFactory, mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _mainFlowId = $"{nameof(ContactMeDialog)}.mainFlow";

            var waterfallSteps = new WaterfallStep[]
            {
                    SendContactsAsync,
                    FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(_mainFlowId, waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = _mainFlowId;
        }

        private async Task<DialogTurnResult> SendContactsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var contactAdaptiveCard = await _mediator.Send(new GetContactsCardQuery());
            var response = MessageFactory.Attachment(contactAdaptiveCard);

            await stepContext.Context.SendActivityAsync(response, cancellationToken);

            return new DialogTurnResult(DialogTurnStatus.Waiting, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(stepContext.Result, cancellationToken);
        }
    }
}
