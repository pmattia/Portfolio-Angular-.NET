import { TestBed } from '@angular/core/testing';

import { PorfolioApiService } from './porfolio-api.service';

describe('PorfolioApiService', () => {
  let service: PorfolioApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PorfolioApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
