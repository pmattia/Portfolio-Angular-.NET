using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Storage.Queries
{
    public class ContentUrlDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
    }
}
