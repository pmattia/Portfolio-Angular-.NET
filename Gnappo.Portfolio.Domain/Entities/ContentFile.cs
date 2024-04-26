using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace Gnappo.Portfolio.Domain.Models
{
    public class ContentFile
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string relativePath { get; set; }
    }
}
