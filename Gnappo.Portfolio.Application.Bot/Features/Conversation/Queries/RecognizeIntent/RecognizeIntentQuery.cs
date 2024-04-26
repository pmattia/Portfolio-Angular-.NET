using Gnappo.Portfolio.Application.Bot.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Bot.Features.Conversation.Queries.RecognizeIntent
{
    public class RecognizeIntentQuery : IRequest<Intent>
    {
        public string text { get; set; }
    }
}
