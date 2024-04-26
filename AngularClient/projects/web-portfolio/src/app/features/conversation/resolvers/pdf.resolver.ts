import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { filter, Observable, take } from 'rxjs';
import { contentNameParam } from '../../../core/contants';
import { ContentUrlDto } from '../../../core/dto/response/content-url.dto';
import { getPdf } from '../store/content/content.actions';
import { ConversationState } from '../store/conversation-state.model';
import { lastContentUrlSelector } from '../store/selectors';

@Injectable({
  providedIn: 'root'
})
export class PdfResolver implements Resolve<ContentUrlDto>  {

  constructor(private store: Store<ConversationState>) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ContentUrlDto> {
    const contentName = route.paramMap.get(contentNameParam) ?? '';
    this.store.dispatch(getPdf({name: contentName}));
    
    return this.store.pipe(select(lastContentUrlSelector))
      .pipe(filter(pdf => pdf != undefined),
        take(1)
      )
  }
}