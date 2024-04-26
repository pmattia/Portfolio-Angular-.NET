import { createAction, props } from '@ngrx/store';
import { ActivityModel, ConnectionStatus } from 'gnappo-lib/bot';

export const startConversation = createAction('[BOT] Start conversation',
    props<{ userId: string }>()
);
export const startConversationSuccess = createAction('[BOT] Start conversation success',
    props<{ connectionStatus: ConnectionStatus }>()
);
export const startConversationFailure = createAction('[BOT] Start conversation failure',
    props<{ error: string, connectionStatus: ConnectionStatus }>()
);

export const getActivities = createAction('[BOT] Get activities');
export const getActivitiesSuccess = createAction('[BOT] Get activities success',
    props<{ activity: ActivityModel }>()
);
export const getActivitiesFailure = createAction('[BOT] Get activities failure',
    props<{ error: string }>()
);

export const sendMessage = createAction('[BOT] Send message',
    props<{ message: string, value?: any }>()
);
export const sendMessageSuccess = createAction('[BOT] Send message success');
export const sendMessageFailure = createAction('[BOT] Send message failure',
    props<{ error: string }>()
);

export const reconnectConversation = createAction('[BOT] Reconnect conversation');
export const reconnectConversationSuccess = createAction('[BOT] Reconnect conversation success',
    props<{ connectionStatus: ConnectionStatus }>()
);
export const reconnectConversationFailure = createAction('[BOT] Reconnect conversation failure',
    props<{ error: string, connectionStatus: ConnectionStatus }>()
);
export const setIsTyping = createAction('[BOT] set is typing',
    props<{ isTyping: boolean }>()
);