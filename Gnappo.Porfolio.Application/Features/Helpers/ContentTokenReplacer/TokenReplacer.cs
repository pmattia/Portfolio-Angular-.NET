using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Helpers.ContentTokenReplacer
{
    public abstract class TokenReplacer
    {
        private readonly Regex _pattern;
        private readonly string _tokenPattern;
        protected TokenReplacer(string token)
        {
            _pattern = new Regex($@"\[{token}=\s*(.*?)\s*\]");
            _tokenPattern = $"[{token}" + "={0}]";
        }
        public abstract string ReplaceTokens(string content);

        protected IEnumerable<string> GetIds(string content)
        {
            var matches = _pattern.Matches(content);
            return matches.Select(m => m.Groups[1].Value);
        }

        protected string ReplaceIdWithUrl(string content, IEnumerable<string> ids, IEnumerable<string> urls)
        {
            foreach (var (id, url) in ids.Zip(urls, (t, u) => (t, u)))
            {
                content = content.Replace(string.Format(_tokenPattern, id), url);
            }

            return content;
        }
    }
}
