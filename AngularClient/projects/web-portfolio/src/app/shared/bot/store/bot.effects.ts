import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { BotService, ConnectionStatus } from "gnappo-lib/bot";
import { catchError, map, mergeMap, of } from "rxjs";
import { getActivities, getActivitiesFailure, getActivitiesSuccess, reconnectConversation, reconnectConversationFailure, reconnectConversationSuccess, sendMessage, sendMessageFailure, sendMessageSuccess, setIsTyping, startConversation, startConversationFailure, startConversationSuccess } from "./bot.actions";

@Injectable()
export class BotEffects{
    startConversation$ = createEffect(() => this.action$.pipe(
        ofType(startConversation),
        mergeMap((obj) => this.botService.startConversation(obj.userId).pipe(
            map((connectionStatus) => {
                if(connectionStatus === ConnectionStatus.Online){
                    return startConversationSuccess({connectionStatus});
                }
                else
                {
                    return startConversationFailure({error: 'Connection failed', connectionStatus})
                }
            }),
            catchError((error) => {
                return of(startConversationFailure({error, connectionStatus: ConnectionStatus.FailedToConnect}));
            })
        ))
      )
    );
    
    activities$ = createEffect(() => this.action$.pipe(
        ofType(getActivities),
        mergeMap(() => this.botService.activity$.pipe(
            map((activity) => {
                return getActivitiesSuccess({activity});
            }),
            catchError((error) => {
                return of(getActivitiesFailure({error}));
            })
        ))
    ));
    
    sendMessage$ = createEffect(() => this.action$.pipe(
        ofType(sendMessage),
        mergeMap((obj) => this.botService.postMessage(obj.message, obj.value).pipe(
            map((id) => {
                return sendMessageSuccess();
            }),
            catchError((error) => {
                return of(sendMessageFailure({error}));
            })
        ))
      )
    );

    reconnectConversation$ = createEffect(() => this.action$.pipe(
        ofType(reconnectConversation),
        mergeMap(() => this.botService.restartConversation().pipe(
            map((connectionStatus) => {
                if(connectionStatus === ConnectionStatus.Online){
                    return startConversationSuccess({connectionStatus});
                }
                else
                {
                    return reconnectConversationFailure({error: 'Connection failed', connectionStatus})
                }
            }),
            catchError((error) => {
                return of(reconnectConversationFailure({error, connectionStatus: ConnectionStatus.FailedToConnect}));
            })
        ))
      )
    );

    isTyping$ = createEffect(() => this.botService.isTyping$.pipe(
        map((isTyping) => {
            return setIsTyping({isTyping});
        })
    ))
    
    constructor(private action$: Actions, private botService: BotService) {
        
    }
}