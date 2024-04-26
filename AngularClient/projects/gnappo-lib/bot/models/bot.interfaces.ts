import { AdaptiveCardSimpleModel } from "./adaptive-card-simple.model";

export declare type UserRole = "bot" | "channel" | "user";
export declare type CardActionTypes = "call" | "downloadFile" | "imBack" | "messageBack" | "openUrl" | "playAudio" | "playVideo" | "postBack" | "signin" | "showImage";

export interface CardAction {
    displayText?: string;
    image: string;
    text?: string;
    title?: string;
    type: CardActionTypes;
    value?: any;
    url?: string;
}
export interface User {
    id: string;
    name?: string;
    iconUrl?: string;
    role?: UserRole;
    isBot: boolean;
    avatar?: string
}
export interface ActivityModel{
    type: string;
    channelData?: any;
    channelId?: string;
    conversation?: Conversation;
    eTag?: string;
    from: User;
    id?: string;
    attachments?: Attachment[];
    //message
    timestamp?: Date;
    text?: string;
    locale?: string;
    textFormat?: "plain" | "markdown" | "xml";
    entities?: any[];
    suggestedActions?: {
        actions: CardAction[];
        to?: string[];
    };
    speak?: string;
    inputHint?: string;
    value?: object;
}
export declare type AttachmentContentType = "application/vnd.microsoft.card.adaptive" | "image/png" | "image/jpg" | "image/jpeg" | "image/gif" | "image/svg+xml" | "audio/mpeg" | "audio/mp4" | "video/mp4";

export interface Attachment {
    type: BotAttachmentType;
    contentType: AttachmentContentType;
    contentUrl?: string;
    adaptiveCard?: AdaptiveCardSimpleModel;
}
export enum BotAttachmentType {
    ContentUrl,
    AdaptiveCard
}

export enum ConnectionStatus {
    Uninitialized = 0,
    Connecting = 1,
    Online = 2,
    ExpiredToken = 3,
    FailedToConnect = 4,
    Ended = 5
}
export interface Conversation {
    conversationId: string;
    token: string;
    eTag?: string;
    streamUrl?: string;
    referenceGrammarId?: string;
}