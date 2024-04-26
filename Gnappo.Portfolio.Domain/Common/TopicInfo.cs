using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Domain.common
{
    public class TopicInfo
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string? CoverFileId { get; set; }
        public IEnumerable<string>? PdfFileIds { get; set; }
    }
}
