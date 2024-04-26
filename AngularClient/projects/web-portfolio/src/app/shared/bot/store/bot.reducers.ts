import { createReducer, on } from '@ngrx/store';
import { ConnectionStatus } from 'gnappo-lib/bot';
import { BotState } from './bot-state.model';
import { getActivities, getActivitiesFailure, getActivitiesSuccess, reconnectConversation, reconnectConversationFailure, reconnectConversationSuccess, sendMessage, sendMessageFailure, sendMessageSuccess, setIsTyping, startConversation, startConversationFailure, startConversationSuccess } from './bot.actions';

export const initialState: BotState = {
    isLoading: false,
    isTyping: false,
    activities: [],
    sentMessages: [],
    connectionStatus: ConnectionStatus.Uninitialized,
    conversationStarted: false
};

export const botReducer = createReducer(
    initialState,
    on(getActivities, (state) => (
        {
            ...state,
            isLoading: true
        }
    )),
    on(getActivitiesSuccess, (state, action) => (
        {
            ...state,
            isLoading: false,
            activities: [...state.activities, action.activity]
        }
    )),
    on(getActivitiesFailure, (state, action) => (
        {
            ...state,
            isLoading: false,
            error: action.error
        }
    )),

    on(startConversation, (state) => (
        {
            ...state,
            isLoading: true,
            conversationStarted: false
        }
    )),
    on(startConversationSuccess, (state, action) => (
        {
            ...state,
            isLoading: false,
            connectionStatus: action.connectionStatus,
            conversationStarted: true
        }
    )),
    on(startConversationFailure, (state, action) => (
        {
            ...state,
            isLoading: false,
            error: action.error,
            connectionStatus: action.connectionStatus
        }
    )),

    on(sendMessage, (state, message) => (
        {
            ...state,
            sentMessages: [...state.sentMessages, message.message],
            isLoading: true
        }
    )),
    on(sendMessageSuccess, (state) => (
        {
            ...state,
            isLoading: false,
        }
    )),
    on(sendMessageFailure, (state, action) => (
        {
            ...state,
            isLoading: false,
            error: action.error
        }
    )),

    on(reconnectConversation, (state) => (
        {
            ...state,
            isLoading: true,
            connectionStatus: ConnectionStatus.Ended,
            activities: [],
            conversationStarted: false
        }
    )),
    on(reconnectConversationSuccess, (state, action) => (
        {
            ...state,
            isLoading: false,
            connectionStatus: action.connectionStatus,
            conversationStarted: true
        }
    )),
    on(reconnectConversationFailure, (state, action) => (
        {
            ...state,
            isLoading: false,
            error: action.error,
            connectionStatus: action.connectionStatus
        }
    )),
    on(setIsTyping, (state, action) => (
        {
            ...state,
            isTyping: action.isTyping
        }
    ))
);