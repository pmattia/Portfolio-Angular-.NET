import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdaptiveCardSimpleComponent } from './adaptive-card-simple.component';

describe('AdaptiveCardSimpleComponent', () => {
  let component: AdaptiveCardSimpleComponent;
  let fixture: ComponentFixture<AdaptiveCardSimpleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdaptiveCardSimpleComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdaptiveCardSimpleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
