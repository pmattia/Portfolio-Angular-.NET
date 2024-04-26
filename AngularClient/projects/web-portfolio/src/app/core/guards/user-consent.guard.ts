import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Observable, map, take } from 'rxjs';
import { AppState } from '../store/models/app-state.model';
import { userConsent } from '../store/selectors';

@Injectable({
  providedIn: 'root'
})
export class UserConsentGuard implements CanActivate {
 
  constructor(private router: Router,private store: Store<AppState>) {
    
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    return this.store.pipe(select(userConsent), take(1), map(consent => {
      if(!consent){
        this.router.navigate([''], { queryParams: { returnUrl: state.url }});
        return false;
      }
      return true;
    }
    ));
  }
  
}
