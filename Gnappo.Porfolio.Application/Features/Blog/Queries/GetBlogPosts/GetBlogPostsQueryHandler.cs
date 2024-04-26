using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Gnappo.Portfolio.Application.Features.Blog.Queries.GetBlogPosts
{
    public class GetBlogPostsQueryHandler : IRequestHandler<GetBlogPostsQuery, BlogPostDto[]>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public GetBlogPostsQueryHandler(IMapper mapper, IBlobService blobService)
        {
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<BlogPostDto[]> Handle(GetBlogPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _blobService.GetBlogPostsAsync(cancellationToken);
            return _mapper.Map<BlogPostDto[]>(posts);
        }
    }
}
