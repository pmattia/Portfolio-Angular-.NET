import { Directive, Input, Output, AfterViewInit, OnChanges, OnDestroy, ElementRef } from '@angular/core';
import * as _ from 'lodash';
import { fromEvent, Observable, Subject, BehaviorSubject } from 'rxjs';
import { takeUntil, debounceTime, map } from 'rxjs/operators';

declare global {
  interface Window { ResizeObserver: any; }
}

@Directive({
  selector: '[appScrollToBottom]'
})
export class ScrollToBottomDirective implements AfterViewInit, OnChanges, OnDestroy {

  @Input('stayAtBottom') stayAtBottom: boolean = true;
  @Input('scrollNow$s') scrollNow: Subject<any> = new Subject();
  @Output('amScrolledToBottom') amScrolledToBottom: Observable<boolean> = new Observable();
  @Input('scrollContainer') scrollContainer?: HTMLElement;

  private destroy$ = new Subject<void>();
  private changes: MutationObserver | undefined;

  private scrollEvent: Observable<void> | undefined;
  private mutations = new BehaviorSubject(null);
  private _userScrolledUp = new BehaviorSubject<boolean>(false);

  private bounceTime = 200;

  constructor(private self: ElementRef) {
    this.destroy$ = new Subject();
  }

  public ngOnChanges() {
    if (this.stayAtBottom === true) {
      this.scrollToBottom(this.bounceTime);
    }
  }

  ngAfterViewInit() {
    this.registerScrollHandlers();
  }

  public ngOnDestroy() {
    this.destroy$.next();
    if (this.changes != null) {
      this.changes.disconnect();
    }
  }

  private registerScrollHandlers() {
    this.amScrolledToBottom = this._userScrolledUp.pipe(takeUntil(this.destroy$), map(x => !x));

    const elementToMonitor = this.scrollContainer || this.self.nativeElement;
    new MutationObserver(() => this.mutations.next(null))
      .observe(elementToMonitor, {
        attributes: true,
        childList: true,
        characterData: true
      });

    this.scrollEvent = fromEvent<void>(this.self.nativeElement, 'scroll')
      .pipe(takeUntil(this.destroy$), debounceTime(100));

    this.scrollNow.pipe(takeUntil(this.destroy$)).subscribe(x => {
      this.scrollToBottom(this.bounceTime);
    });

    this.mutations.pipe(takeUntil(this.destroy$)).subscribe(x => {
    
      if (this._userScrolledUp.value === false) {
        this.scrollToBottom(this.bounceTime);
      }
    });

    this.scrollEvent.pipe(takeUntil(this.destroy$)).subscribe(x => {
      this.setHasUserScrolledUp();
    });
  }

  private setHasUserScrolledUp() {
    // If currently at bottom, user not scrolled up;
    const el = this.self.nativeElement;
    const bottomScrolled = Math.abs(el.scrollHeight - (el.clientHeight + el.scrollTop)) < 1;
    
    if (bottomScrolled === true) {
      this._userScrolledUp.next(false);
      return;
    }

    // If, already userScrolledUp, nothing to do.
    if (this._userScrolledUp.value === true) {
      return;
    }

    // Not at bottom, is caused by mutation or user?
    // Assume mutation will scroll within 5ms.
    setTimeout(() => {
      if (bottomScrolled === false) {
        this._userScrolledUp.next(true);
      }
    }, 5);
  }

  scrollToBottom(timeout: number = 0) {
    _.debounce(() => {
      this.self.nativeElement.scrollTop = this.self.nativeElement.scrollHeight;
    }, timeout).call(this);
  }





}