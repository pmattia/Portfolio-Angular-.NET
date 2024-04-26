import { NgModule } from '@angular/core';
import { PorfolioApiService } from './services/porfolio-api.service';
import { HttpInterceptorService } from './interceptors/http-interceptor.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { UserConsentGuard } from './guards/user-consent.guard';
import { AppStateGuard } from './guards/app-state.guard';
import { LocalStorageService } from './services/local-storage.service';

@NgModule({
  providers: [
    PorfolioApiService,
    HttpInterceptorService,
    LocalStorageService,
    UserConsentGuard,
    AppStateGuard,
    { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true }
  ]
})
export class CoreModule { }
