import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Observable, map, take } from 'rxjs';
import { AppState } from '../store/models/app-state.model';
import { appSelector } from '../store/selectors';

@Injectable({
  providedIn: 'root'
})
export class AppStateGuard implements CanActivate {
  constructor(private store: Store<AppState>) {
    
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.store.pipe(select(appSelector), take(1), map(state => {
      return !!state;
    }));
  }
  
}
