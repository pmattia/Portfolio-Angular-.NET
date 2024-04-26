import { Attachment, CardActionTypes, User } from "./bot.interfaces";

export interface UserMessagesModel{
    from: User,
    activities: {
        id: string;
        suggestedActions?: {
            actions: UserMessageCardAction[];
            to?: string[];
        };
        timestamp: Date;
        text?: string;
        attachments?: Attachment[];
    }[]
}

export interface UserMessageCardAction{
    displayText?: string;
    image: string;
    text: string;
    title?: string;
    type: CardActionTypes;
    value?: any;
    disabled: boolean
}