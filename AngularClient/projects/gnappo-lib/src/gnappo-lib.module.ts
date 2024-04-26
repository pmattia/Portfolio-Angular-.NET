import { NgModule } from '@angular/core';
import { CourtesyPageComponent } from './courtesy-page/courtesy-page.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import {AvatarModule} from 'primeng/avatar';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from './loader/loader.component';
import {ProgressBarModule} from 'primeng/progressbar';
import { ButtonModule } from 'primeng/button';


@NgModule({
  declarations: [
    CourtesyPageComponent,
    ErrorPageComponent,
    LoaderComponent
  ],
  imports: [
    CommonModule,
    AvatarModule,
    ProgressBarModule,
    ButtonModule
  ],
  exports: [
    CourtesyPageComponent,
    ErrorPageComponent,
    LoaderComponent
  ]
})
export class GnappoLibModule { }
