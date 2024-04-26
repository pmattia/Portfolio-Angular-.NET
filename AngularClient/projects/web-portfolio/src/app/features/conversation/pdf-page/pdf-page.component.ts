import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { delay, map, Observable, tap } from 'rxjs';
import { ContentUrlDto } from '../../../core/dto/response/content-url.dto';
import { FeatureComponent } from '../../../core/feature-component';
import { AppState } from '../../../core/store/models/app-state.model';
import { conversate, openSidePanel } from '../../../core/store/actions/ui.actions';

@Component({
  templateUrl: './pdf-page.component.html',
  styleUrls: ['./pdf-page.component.scss']
})
export class PdfPageComponent extends FeatureComponent implements OnInit {
  content$: Observable<ContentUrlDto>;
  
  constructor(store: Store<AppState>, private route: ActivatedRoute) { 
    super(store);
    this.content$ = this.route.data
    .pipe(
      delay(1000),
      map(data => {
        const modalProxy = (data['contentModel'] as ContentUrlDto);
        return modalProxy;
      }),
      tap(() => {
        this.store.dispatch(openSidePanel())
      })
    );
  }

  ngOnInit(): void {
  }

  downloadPdf(url: string){
    var ticks = ((new Date().getTime() * 10000) + 621355968000000000);
    window.open(`${url}`);
  }

  closeContentPage(){
    this.store.dispatch(conversate());
  }
}
