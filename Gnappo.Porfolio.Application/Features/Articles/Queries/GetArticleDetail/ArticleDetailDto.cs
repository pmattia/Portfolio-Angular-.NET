using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetArticleDetail
{
    public class ArticleDetailDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
