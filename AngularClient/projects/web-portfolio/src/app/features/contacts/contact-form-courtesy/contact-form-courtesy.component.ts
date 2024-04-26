import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { Observable, of, tap } from 'rxjs';
import { ContactMessageDto } from '../../../core/dto/request/contact-message.dto';
import { FeatureComponent } from '../../../core/feature-component';
import { conversate, openSidePanel, showErrorPage } from '../../../core/store/actions/ui.actions';
import { AppState } from '../../../core/store/models/app-state.model';
import { lastContactMessageSelector } from '../store/selectors';

@Component({
  templateUrl: './contact-form-courtesy.component.html',
  styleUrls: ['./contact-form-courtesy.component.scss']
})
export class ContactFormCourtesyComponent extends FeatureComponent implements OnInit {

  title = $localize`Request sent successfully`;
  text = $localize`Thank you very much {name} for your contact request!`;
  contactMessage$: Observable<ContactMessageDto>;
  actions: { label: string, action: () => void }[];

  constructor(store: Store<AppState>) {
    super(store);
    this.contactMessage$ = this.store.pipe(select(lastContactMessageSelector))
      .pipe(tap(contactMessage => {
        if (contactMessage) {
          this.text = this.text.replace('{name}', contactMessage.name);
        } else {

          this.store.dispatch(conversate());
        }
      }));
    this.actions = [
      {
        label: `Chiudi`,
        action: () => {
          this.closeContentPage();
        }
      }
    ];


  }

  ngOnInit(): void {
    this.store.dispatch(openSidePanel());
  }

  closeContentPage() {
    this.store.dispatch(conversate());
  }
}
