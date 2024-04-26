import { TestBed } from '@angular/core/testing';

import { UserConsentGuard } from './user-consent.guard';

describe('UserConsentGuard', () => {
  let guard: UserConsentGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(UserConsentGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
