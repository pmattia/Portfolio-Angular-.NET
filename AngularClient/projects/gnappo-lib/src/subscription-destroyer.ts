import { Injectable, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable()
export abstract class SubscriptionDestroyer implements OnDestroy {
  private subscriptions = new Subscription();
  
  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  public add$<T>(sub: Subscription): void {
    this.subscriptions.add(sub);
  }
}
