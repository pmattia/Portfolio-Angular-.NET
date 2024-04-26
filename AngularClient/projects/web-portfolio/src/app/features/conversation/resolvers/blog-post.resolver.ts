import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { filter, Observable, take } from 'rxjs';
import { contentNameParam } from '../../../core/contants';
import { ArticleDto } from '../../../core/dto/response/article.dto';
import { getBlogPost } from '../store/content/content.actions';
import { ConversationState } from '../store/conversation-state.model';
import { lastBlogPostSelector } from '../store/selectors';

@Injectable({
  providedIn: 'root'
})
export class BlogPostResolver implements Resolve<ArticleDto>  {

  constructor(private store: Store<ConversationState>) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ArticleDto> {
    const contentName = route.paramMap.get(contentNameParam) ?? '';
    this.store.dispatch(getBlogPost({name: contentName}));
    
    return this.store.pipe(select(lastBlogPostSelector))
      .pipe(filter(blogPost => blogPost != undefined),
        take(1)
      )
  }
}
