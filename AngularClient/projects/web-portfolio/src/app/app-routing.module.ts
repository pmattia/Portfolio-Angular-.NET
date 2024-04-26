import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorPageComponent } from 'gnappo-lib';
import { routeConversationUrl, routeErrorUrl, routeGreetingUrl } from './core/contants';
import { UserConsentGuard } from './core/guards/user-consent.guard';
import { AppStateGuard } from './core/guards/app-state.guard';
import { ErrorPageResolver } from './core/resolvers/error-page.resolver';

const routes: Routes = [
  {
    path: routeGreetingUrl,
    canActivate: [UserConsentGuard, AppStateGuard],
    loadChildren: () => import('./features/greeting/greeting.module').then(m => m.GreetingModule),
    // resolve: { ready: InitializeReportsResolver},
    data: { animation: 'greeting' }
  },
  {
    path: routeConversationUrl,
    canActivate: [UserConsentGuard, AppStateGuard],
    loadChildren: () => import('./features/conversation/conversation.module').then(m => m.ConversationModule),
    // resolve: { ready: InitializeReportsResolver},
    data: { animation: 'conversation' }
  },
  {
    path: routeErrorUrl,
    component: ErrorPageComponent,
    resolve: {
      data: ErrorPageResolver,
    }
  },
  {
    path: '404',
    component: ErrorPageComponent,
    resolve: {
      data: ErrorPageResolver,
    }
  },
  { 
    path: '',
    loadComponent: () => import('./shared/user-consent/user-consent.component').then(mod => mod.UserConsentComponent),
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/404'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes/*, { enableTracing: true }*/)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
