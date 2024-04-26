import { Store } from '@ngrx/store';
import { SubscriptionDestroyer } from 'gnappo-lib';
import { AppState } from './store/models/app-state.model';

/** Base contract for a component. Provides functions to manage observables and expose configuration and session */
export abstract class FeatureComponent extends SubscriptionDestroyer {
   public store: Store<AppState>;

  constructor(store: Store<AppState>) {
    super();
    this.store = store;
  }
}
