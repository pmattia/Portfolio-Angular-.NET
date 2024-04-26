import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, map, tap } from 'rxjs';
import { ArticleDto } from '../../../core/dto/response/article.dto';
import { BlogPostDto } from '../../../core/dto/response/blog-post.dto';
import { FeatureComponent } from '../../../core/feature-component';
import { conversate, openSidePanel } from '../../../core/store/actions/ui.actions';
import { AppState } from '../../../core/store/models/app-state.model';

@Component({
  selector: 'app-content-page',
  templateUrl: './content-page.component.html',
  styleUrls: ['./content-page.component.scss']
})
export class ContentPageComponent extends FeatureComponent implements OnInit, AfterViewInit {

  content$: Observable<ArticleDto | BlogPostDto>;
  private dynamicContentMutationObserver!: MutationObserver;
  @ViewChild('dynamicContent') dynamicContent!: ElementRef;

  constructor(store: Store<AppState>, private route: ActivatedRoute, private router: Router, private element: ElementRef) {
    super(store);
    this.content$ = this.route.data
      .pipe(
        map(data => {
          const modalProxy = (data['contentModel']);
          return modalProxy;
        }),
        tap(() => {
          this.element.nativeElement.scrollTop = 0; //always scroll to top on new content
          this.store.dispatch(openSidePanel());
        })
      );
  }

  ngOnInit(): void {

  }

  ngAfterViewInit(): void {
    this.preventHrefBehaviour()
    this.dynamicContentMutationObserver = new MutationObserver(mutations => {
      mutations.forEach(mutation => {
        if (mutation.type === 'childList') {
          this.preventHrefBehaviour();
        }
      });
    });

    this.dynamicContentMutationObserver.observe(this.dynamicContent.nativeElement, { childList: true, subtree: true });
  }

  private preventHrefBehaviour() {
    const anchorElements = this.dynamicContent.nativeElement.querySelectorAll('a');
    anchorElements?.forEach((a: HTMLElement) => {
      a.addEventListener('click', (e: MouseEvent) => {
        if (a.getAttribute('target') as string !== '_blank') {
          e.preventDefault();
          
          if (this.isAbsoluteUrl(a.getAttribute('href') as string)) {
            window.open(a.getAttribute('href') as string, '_blank');
          } else {
            this.router.navigateByUrl(a.getAttribute('href') as string);
            // this.store.dispatch(showArticle({ articleName: 'ux-design' }));
          }
        }
      });
    });
  }

  closeContentPage() {
    this.store.dispatch(conversate());
  }

  private isAbsoluteUrl(url: string): boolean {
    var pat = /^https?:\/\//i;
    return pat.test(url)
  }

  override ngOnDestroy(): void {
    this.dynamicContentMutationObserver.disconnect();
    super.ngOnDestroy();
  }
}
