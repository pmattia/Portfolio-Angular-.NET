import { ActivityModel, ConnectionStatus } from "gnappo-lib/bot";

export interface BotState{
    isLoading: boolean;
    isTyping: boolean;
    activities: ActivityModel[];
    sentMessages: string[];
    connectionStatus: ConnectionStatus;
    conversationStarted: boolean;
}