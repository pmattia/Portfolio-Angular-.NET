﻿namespace Gnappo.Portfolio.Application.Bot.Models
{
    public class ConversationData
    {
        // Track whether we have already asked the user's name
        public bool PromptedUserForName { get; set; } = false;
    }
}
