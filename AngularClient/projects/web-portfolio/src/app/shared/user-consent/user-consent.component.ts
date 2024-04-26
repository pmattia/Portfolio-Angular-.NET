import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { GnappoLibModule } from 'gnappo-lib';
import { Observable, tap } from 'rxjs';
import { FeatureComponent } from '../../core/feature-component';
import { AppState } from '../../core/store/models/app-state.model';
import { userAcceptConsent, userRefuseConsent } from '../../core/store/actions/app.actions';
import { userConsent } from '../../core/store/selectors';
import { greeting, showErrorPage } from '../../core/store/actions/ui.actions';
import { privacyPolicyUrl } from '../../core/contants';

@Component({
  selector: 'app-user-consent',
  templateUrl: './user-consent.component.html',
  styleUrls: ['./user-consent.component.scss'],
  standalone: true,
  imports: [GnappoLibModule, CommonModule]
})
export class UserConsentComponent extends FeatureComponent {

  actions: { label: string, action: () => void }[];
  userConsent$: Observable<boolean>;
  privacyPolicyPageUrl: string;

  constructor(@Inject(privacyPolicyUrl) privacyPolicyUrl: string, private route: ActivatedRoute, private router: Router, store: Store<AppState>){
    super(store);

    this.privacyPolicyPageUrl = privacyPolicyUrl;
    this.userConsent$ = this.store.pipe(select(userConsent))
    .pipe(tap(consent => {
        if (consent) {
          const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl')
          this.AccessPortfolio(returnUrl);
        }
      }
    ));
      
    

    this.actions = [
      {
        label: $localize`Accept`,
        action: () => {
          this.store.dispatch(userAcceptConsent());
        }
      },
      {
        label: $localize`Reject`,
        action: () => {
          this.store.dispatch(userRefuseConsent());
          this.store.dispatch(showErrorPage({ error: 'fuck you!' }));
        }
      }
    ];
  }
  
  private AccessPortfolio(url: string | null){
    if (url) {
      this.router.navigateByUrl(url);
    }
    else{
      this.store.dispatch(greeting());
    }
  }

}
