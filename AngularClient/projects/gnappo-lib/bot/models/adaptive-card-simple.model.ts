import { CardAction } from "./bot.interfaces"

export interface AdaptiveCardSimpleModel {
    actions: CardAction[],
    body: {
        type: string,
        url: string
    }[]
}