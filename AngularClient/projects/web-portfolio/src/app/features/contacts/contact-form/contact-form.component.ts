import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import { SelectItem } from 'primeng/api';
import { Observable } from 'rxjs';
import { ContactMessageDto } from '../../../core/dto/request/contact-message.dto';
import { FeatureComponent } from '../../../core/feature-component';
import { AppState } from '../../../core/store/models/app-state.model';
import { conversate, openSidePanel } from '../../../core/store/actions/ui.actions';
import { sendContactMessage } from '../store/contact/contact.actions';
import { ContactMessageSendingSelector } from '../store/selectors';
import { privacyPolicyUrl } from '../../../core/contants';

@Component({
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss']
})
export class ContactFormComponent extends FeatureComponent implements OnInit {
  reasons: SelectItem[];
  contactForm = new FormGroup({
    name: new FormControl('', Validators.required),
    reason: new FormControl(''),
    email: new FormControl('', [Validators.required, Validators.email]),
    message: new FormControl('', Validators.required)
  });
  isSending$: Observable<boolean>;
  privacyPolicyPageUrl: string;

  constructor(@Inject(privacyPolicyUrl) privacyPolicyUrl: string, store: Store<AppState>) {
    super(store);
    this.privacyPolicyPageUrl = privacyPolicyUrl;
    this.reasons = [ 
      { label: $localize`I'm doing recuiting`, value: 'Sto facendo recuiting' },
      { label: $localize`Professional interest`, value: 'Interesse professionale' },
      { label: $localize`Personal interest`, value: 'Interesse personale' },
      { label: $localize`I clicked a random link`, value: 'Ho cliccato un link a caso' },
      { label: $localize`Other`, value: 'Altro' }
    ];

    this.isSending$ = this.store.pipe(select(ContactMessageSendingSelector));
  }

  ngOnInit(): void {
    this.store.dispatch(openSidePanel());
  }
  
  onSubmit(): void {
    const contactMessage: ContactMessageDto = {
      email: this.contactForm.controls['email'].value!,
      message: this.contactForm.controls['message'].value!,
      name: this.contactForm.controls['name'].value!,
      reason: (this.contactForm.controls['reason'].value! as any).value,
    }
    
    this.store.dispatch(sendContactMessage({ contactMessage }));
  }

  closeContentPage() {
    this.store.dispatch(conversate());
  }
}