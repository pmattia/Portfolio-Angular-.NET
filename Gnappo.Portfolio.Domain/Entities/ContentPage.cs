using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Domain.Models
{
    public abstract class ContentPage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string BlobName { get; set; }
        public string CoverUrl { get; set; }
        public int Order { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public Boolean Published { get; set; }
        public string? AssistantFileId { get; set; }
    }
}
