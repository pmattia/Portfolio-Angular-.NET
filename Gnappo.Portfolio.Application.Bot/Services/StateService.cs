﻿using Gnappo.Portfolio.Application.Bot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;

namespace Gnappo.Portfolio.Application.Bot.Services
{
    public class StateService
    {
		#region Variables

		// State Variables
		public ConversationState ConversationState { get; }
		public UserState UserState { get; }

		// IDs
		public static string UserProfileId { get; } = $"{nameof(StateService)}.UserProfile";
		public static string ConversationDataId { get; } = $"{nameof(StateService)}.ConversationData";
		public static string DialogStateId { get; } = $"{nameof(StateService)}.DialogState";

		// Accessors
		public IStatePropertyAccessor<UserData> UserProfileAccessor { get; set; }
		public IStatePropertyAccessor<ConversationData> ConversationDataAccessor { get; set; }
		public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }

		#endregion

		public StateService(UserState userState, ConversationState conversationState)
		{
			ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
			UserState = userState ?? throw new ArgumentNullException(nameof(userState));

			InitializeAccessors();
		}
		private void InitializeAccessors()
		{
			// Initialize Conversation State Accessors
			ConversationDataAccessor = ConversationState.CreateProperty<ConversationData>(ConversationDataId);
			DialogStateAccessor = ConversationState.CreateProperty<DialogState>(DialogStateId);

			// Initialize User State
			UserProfileAccessor = UserState.CreateProperty<UserData>(UserProfileId);
		}
    }
}
