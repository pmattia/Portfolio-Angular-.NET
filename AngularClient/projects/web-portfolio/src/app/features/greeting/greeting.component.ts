import { Component, Inject, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ActivityModel, CardAction } from 'gnappo-lib/bot';
import { Observable, filter, first, tap } from 'rxjs';
import { AppState } from '../../core/store/models/app-state.model';
import { conversate } from '../../core/store/actions/ui.actions';
import { BotComponent } from '../../shared/bot/bot-component';
import { botActivitiesSelector } from '../../shared/bot/store/selectors';
import { sendMessage } from '../../shared/bot/store/bot.actions';
import { USERID } from '../../core/contants';

@Component({
  selector: 'app-greeting',
  templateUrl: './greeting.component.html',
  styleUrls: ['./greeting.component.scss']
})
export class GreetingComponent extends BotComponent implements OnInit {
  activities$: Observable<ActivityModel[]>;

  constructor(store: Store<AppState>, @Inject(USERID) userId: string) {
    super(store);
    this.initBot(userId,true);
    this.activities$ = this.store.pipe(select(botActivitiesSelector))
                                  .pipe(filter((activities) => activities.length > 0), first());
  }

  ngOnInit(): void {
  }

  handleSuggestedAction(action: CardAction) {
    switch (action.type) {
      case 'messageBack':
        this.store.dispatch(sendMessage({ message: action.text!, value: action.value }))
        this.store.dispatch(conversate());
        break;
      default:
        //todo
        break;
    }
  }
}
