import { Directive, HostListener } from '@angular/core';
import { AppState } from '../../core/store/models/app-state.model';
import { Store } from '@ngrx/store';
import { conversate } from '../../core/store/actions/ui.actions';

@Directive({
  selector: '[conversateOnBack]'
})
export class ConversateOnBackDirective {

  constructor(private store: Store<AppState>) { }

  @HostListener('window:popstate', ['$event'])
  onPopState(event:any) {
    this.store.dispatch(conversate());
  }
}
