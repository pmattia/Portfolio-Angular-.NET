import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, catchError, of, tap } from 'rxjs';
import { AppState } from '../store/models/app-state.model';
import { showErrorPage } from '../store/actions/ui.actions';

@Injectable({
  providedIn: 'root'
})
export class HttpInterceptorService implements HttpInterceptor {
  requestCount = 0;

  constructor(private store: Store<AppState>) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .pipe(

        tap((item) => {
          // console.log('item', item);
        }),
        catchError((err: any) => {
  
          if (err instanceof HttpErrorResponse) {
            this.store.dispatch(showErrorPage(err.error));
          }
          return of(err);
        })
      );
  }
}
