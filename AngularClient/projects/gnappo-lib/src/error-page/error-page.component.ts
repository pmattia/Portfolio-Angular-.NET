import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, map } from 'rxjs';
import { ErrorPageModel } from './error-page.model';

@Component({
  selector: 'g-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.scss']
})
export class ErrorPageComponent {
  model$: Observable<ErrorPageModel>;
  constructor(private route:ActivatedRoute) {
    this.model$ = this.route.data.pipe(
      map(data => {
        return data['data'] as ErrorPageModel;
      }),
    );
   }
}
