using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters
{
    /// <summary>
    /// for articles and blog posts
    /// </summary>
    public class ContentPageUrlFormatter : IUrlFormatter
    {
        private readonly string _contentUrlPattern;
        private readonly string _webPortfolioUrl;

        public ContentPageUrlFormatter(string contentUrlPattern, string webPortfolioUrl)
        {
            _contentUrlPattern = contentUrlPattern;
            _webPortfolioUrl = webPortfolioUrl;
        }

        public string Format(string id)
        {
            return string.Format(_contentUrlPattern, _webPortfolioUrl, id);
        }

        public string FormatByPattern(string urlPattern)
        {
            throw new NotImplementedException();
        }
    }
}
