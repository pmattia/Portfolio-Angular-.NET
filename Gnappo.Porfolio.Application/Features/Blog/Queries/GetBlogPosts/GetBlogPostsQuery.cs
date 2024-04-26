using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Blog.Queries.GetBlogPosts
{
    public class GetBlogPostsQuery : IRequest<BlogPostDto[]>
    {
    }
}
