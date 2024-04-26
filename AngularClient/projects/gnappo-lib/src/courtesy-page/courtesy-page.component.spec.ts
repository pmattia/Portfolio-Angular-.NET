import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourtesyPageComponent } from './courtesy-page.component';

describe('CourtesyPageComponent', () => {
  let component: CourtesyPageComponent;
  let fixture: ComponentFixture<CourtesyPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CourtesyPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourtesyPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
