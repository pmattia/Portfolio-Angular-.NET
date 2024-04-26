import { TestBed } from '@angular/core/testing';

import { ErrorPageResolver } from './error-page.resolver';

describe('ErrorPageResolver', () => {
  let resolver: ErrorPageResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(ErrorPageResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
