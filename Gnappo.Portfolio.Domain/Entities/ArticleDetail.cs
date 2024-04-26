using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Domain.Models
{
    public class ArticleDetail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }

    }
}
