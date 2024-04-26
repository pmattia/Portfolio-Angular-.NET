import { TestBed } from '@angular/core/testing';

import { PdfResolver } from './pdf.resolver';

describe('PdfResolverService', () => {
  let service: PdfResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PdfResolver);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
