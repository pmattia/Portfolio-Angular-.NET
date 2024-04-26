import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EffectsModule } from '@ngrx/effects';
import { META_REDUCERS, StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { BurgerModule } from 'gnappo-lib/burger';
import * as _ from 'lodash';
import { RippleModule } from 'primeng/ripple';
import { SpeedDialModule } from 'primeng/speeddial';
import { environment } from '../environments/environment';
import { UiEffects } from './core/store/effects/ui.effects';
import { reducers } from './core/store/reducers';
import { storageMetaReducer } from './core/store/storage.metareducer';
import { LocalStorageService } from './core/services/local-storage.service';
import { USERID, criptoKey, privacyPolicyUrl, webPortfolioApi } from './core/contants';
import { BOT_API_SECRET } from 'gnappo-lib';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    CoreModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    SpeedDialModule,
    RippleModule,
    BurgerModule,
    StoreModule.forRoot(reducers),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: environment.production }),
    EffectsModule.forRoot([UiEffects])
  ],
  providers:
    [
      {
        provide: META_REDUCERS,
        deps: [LocalStorageService],
        useFactory: storageMetaReducer,
        multi: true
      },
      {
        provide: USERID, useFactory: () => {
          const idToHash = _.uniqueId(); //window.navigator.userAgent;
          var hash = 0;
          for (var i = 0; i < idToHash.length; i++) {
            var code = idToHash.charCodeAt(i);
            hash = ((hash << 5) - hash) + code;
            hash = hash & hash; // Convert to 32bit integer
          }
          return hash.toString();
        }
      },
      {
        provide: BOT_API_SECRET, useValue: environment.directLineSecret
      },
      {
        provide: webPortfolioApi, useValue: environment.webApiUrl
      },
      {
        provide: criptoKey, useValue: environment.cryptoKey
      },
      {
        provide: privacyPolicyUrl, useValue: environment.privacyPolicyUrl
      }
    ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent]
})
export class AppModule { }
