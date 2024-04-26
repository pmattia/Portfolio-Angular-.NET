using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Gnappo.Portfolio.Application.Features.Helpers.ContentTokenReplacer
{
    public class ContentPageTokenReplacer : TokenReplacer
    {
        private readonly IUrlFormatter _formatter;
        public ContentPageTokenReplacer(string token, IUrlFormatter formatter) : base(token)
        {
            _formatter = formatter;
        }
        public override string ReplaceTokens(string content)
        {
            var articleIds = GetIds(content);
            var articleUrls = new List<string>();
            foreach (var articleId in articleIds)
            {
                articleUrls.Add(_formatter.Format(articleId));
            }

            return ReplaceIdWithUrl(content, articleIds, articleUrls);
        }
    }
}
