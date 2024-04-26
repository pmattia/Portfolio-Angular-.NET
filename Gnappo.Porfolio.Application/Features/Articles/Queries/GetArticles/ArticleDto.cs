using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetArticleDetail
{
    public class ArticleDto
    {
        public string Title { get; set; }
        public string BlobName { get; set; }
        public string CoverUrl { get; set; }
        public int Order { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
