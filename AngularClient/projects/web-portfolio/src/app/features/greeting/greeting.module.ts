import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { GnappoLibModule } from 'gnappo-lib';
import { ButtonModule } from 'primeng/button';
import { SharedModule } from '../../shared/shared.module';
import { GreetingComponent } from './greeting.component';


@NgModule({
  declarations: [
    GreetingComponent
  ],
  imports: [
    CommonModule,
    ButtonModule,
    GnappoLibModule,
    SharedModule,
    RouterModule.forChild([
      {
        path: '',
        component: GreetingComponent,
        // canActivate: [CanActivateBot],
      }
      
    ])
  ]
})
export class GreetingModule { }
