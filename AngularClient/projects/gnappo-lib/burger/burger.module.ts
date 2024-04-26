import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { BurgerComponent } from './burger.component';
import {SidebarModule} from 'primeng/sidebar';


@NgModule({
  declarations: [
    BurgerComponent
  ],
  imports: [
    CommonModule,
    ButtonModule,
    RippleModule,
    SidebarModule
  ],
  exports: [
    BurgerComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class BurgerModule { }
