using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Blog.Queries.GetBlogPosts
{
    public class BlogPostDto
    {
        public string Title { get; set; }
        public string BlobName { get; set; }
        public string CoverUrl { get; set; }
        public int Order { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
