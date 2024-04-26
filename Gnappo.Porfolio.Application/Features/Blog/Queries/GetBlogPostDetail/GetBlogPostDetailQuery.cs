using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Blog.Queries.GetBlogPostDetail
{
    public class GetBlogPostDetailQuery : IRequest<BlogPostDetailDto>
    {
        public string Name { get; set; }
    }
}
