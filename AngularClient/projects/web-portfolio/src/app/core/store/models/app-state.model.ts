import { UiState } from "./ui-state.model";
import { UserState } from "./user-state.model";

export interface AppState{
    ui: UiState
    user: UserState
    // bot: BotState
    // conversation: ConversationState
}