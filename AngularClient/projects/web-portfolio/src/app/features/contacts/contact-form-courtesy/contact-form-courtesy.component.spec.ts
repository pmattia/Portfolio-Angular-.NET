import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactFormCourtesyComponent } from './contact-form-courtesy.component';

describe('ContactFormCourtesyComponent', () => {
  let component: ContactFormCourtesyComponent;
  let fixture: ComponentFixture<ContactFormCourtesyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContactFormCourtesyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContactFormCourtesyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
