import { createFeatureSelector, createSelector } from "@ngrx/store";
import { BotState } from "./bot-state.model";

const selectState = createFeatureSelector<BotState>('bot');

export const botActivitiesSelector = createSelector(selectState, (state) => state.activities)
export const botLastActivitySelector = createSelector(selectState, (state) => state.activities[state.activities.length - 1])
export const botStatusSelector = createSelector(selectState, (state) => state.connectionStatus)
export const botConversationStartedSelector = createSelector(selectState, (state) => state.conversationStarted && state.activities.length > 0)
export const botSentMessagesSelector = createSelector(selectState, (state) => state.sentMessages)
export const botIsTypingSelector = createSelector(selectState, (state) => state.isTyping)