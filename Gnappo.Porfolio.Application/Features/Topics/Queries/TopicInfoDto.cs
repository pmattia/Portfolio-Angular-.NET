using Gnappo.Portfolio.Application.Contracts.Domain;
using Gnappo.Portfolio.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Topics.Queries
{
    public class TopicInfoDto
    {
        public string Title { get; set; }

        public PortfolioContentTypeEnum Type { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string? CoverFileId { get; set; }
        public IEnumerable<string>? PdfFileIds { get; set; }
    }
}
