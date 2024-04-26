import { TestBed } from '@angular/core/testing';

import { BlogPostResolver } from './blog-post.resolver';

describe('BlogPostResolverService', () => {
  let service: BlogPostResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BlogPostResolver);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
