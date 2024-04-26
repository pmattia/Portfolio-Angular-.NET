import { Store, select } from "@ngrx/store";
import { ConnectionStatus } from "gnappo-lib/bot";
import { take } from "rxjs";
import { FeatureComponent } from "../../core/feature-component";
import { AppState } from "../../core/store/models/app-state.model";
import { showErrorPage } from "../../core/store/actions/ui.actions";
import { getActivities, reconnectConversation, startConversation } from "./store/bot.actions";
import { botConversationStartedSelector, botStatusSelector } from "./store/selectors";

export abstract class BotComponent extends FeatureComponent {

    constructor(store: Store<AppState>) {
        super(store);
    }
    initBot(userId: string, forceRestart = false) {
        this.add$(this.store.pipe(select(botConversationStartedSelector))
            .pipe(take(1))
            .subscribe(isStarted => {
                if (!isStarted) {
                    this.store.dispatch(startConversation({ userId: userId }));
                    this.store.dispatch(getActivities());
                } else {
                    if (forceRestart) {
                        this.store.dispatch(reconnectConversation());
                    }
                }
            }
            ));

        this.add$(this.store.pipe(
            select(botStatusSelector))
            .subscribe((status) => {
                if (status != ConnectionStatus.Online
                    && status != ConnectionStatus.Connecting
                    && status != ConnectionStatus.Uninitialized) {
                    console.error('BOT OFFLINE');
                    this.store.dispatch(showErrorPage({ error: 'BOT OFFLINE' }));
                }
            })
        );
    }
}