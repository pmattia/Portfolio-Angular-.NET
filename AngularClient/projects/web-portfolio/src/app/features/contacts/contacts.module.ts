import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { GnappoLibModule } from 'gnappo-lib';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { TagModule } from 'primeng/tag';
import { routeSuccessUrl } from '../../core/contants';
import { SharedModule } from '../../shared/shared.module';
import { ContactFormCourtesyComponent } from './contact-form-courtesy/contact-form-courtesy.component';
import { ContactFormComponent } from './contact-form/contact-form.component';
import { ContactEffects } from './store/contact/contact.effects';
import { contactReducer } from './store/contact/contact.reducers';


@NgModule({
  declarations: [
    ContactFormComponent,
    ContactFormCourtesyComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    InputTextModule,
    ButtonModule,
    InputTextareaModule,
    DropdownModule,
    TagModule,
    ReactiveFormsModule,
    SharedModule,
    GnappoLibModule,
    StoreModule.forFeature('contacts', contactReducer),
    EffectsModule.forFeature([ContactEffects]),
    RouterModule.forChild([
      {
        path: '',
        children: [
          {
            path: '',
            component: ContactFormComponent,
          },
          {
            path: routeSuccessUrl,
            component: ContactFormCourtesyComponent
          }
        ]
      }
    ])
  ],
  providers: [ContactEffects]
})
export class ContactsModule { }
