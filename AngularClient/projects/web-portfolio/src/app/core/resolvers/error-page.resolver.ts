import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { ConversationState } from '../../features/conversation/store/conversation-state.model';
import { Store } from '@ngrx/store';
import { conversate } from '../store/actions/ui.actions';
import { ErrorPageModel } from 'gnappo-lib';

@Injectable({
  providedIn: 'root'
})
export class ErrorPageResolver implements Resolve<ErrorPageModel> {
  constructor(private store: Store<ConversationState>) { }
  resolve() {
    const actions = [
      {
        label: $localize`Restart`,
        action: () => {
          this.store.dispatch(conversate());
        }
      }
    ];
    return of({ 
      title: $localize`OOOOpppsss........`, 
      text: $localize`Something went wrong!`, 
      actions: actions
    });
  }
}
