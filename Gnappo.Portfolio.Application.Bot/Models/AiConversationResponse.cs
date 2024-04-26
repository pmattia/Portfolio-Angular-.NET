using Gnappo.Portfolio.Domain.Models;
using Microsoft.Bot.Schema;
using System.Collections.Generic;

namespace Gnappo.Portfolio.Application.Bot.Models
{
    public class AiConversationResponse
    {
        public string Text { get; set; }
        public bool Success { get; set; }
        public Attachment Suggestions { get; set; }
    }
}
