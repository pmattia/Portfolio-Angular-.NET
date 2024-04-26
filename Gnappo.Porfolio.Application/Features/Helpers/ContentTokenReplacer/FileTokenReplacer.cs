using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using Gnappo.Portfolio.Domain.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Helpers.ContentTokenReplacer
{
    public class FileTokenReplacer : TokenReplacer
    {
        private readonly IBlobService _blobService;
        private readonly IUrlFormatter _formatter;
        public FileTokenReplacer(string token, IBlobService blobService, IUrlFormatter formatter) : base(token)
        {
            _blobService = blobService;
            _formatter = formatter;
        }
        public override string ReplaceTokens(string content)
        {
            var fileIds = GetIds(content);
            var fileUrls = new List<string>();
            foreach (var fileId in fileIds)
            {
                var image = _blobService.GetContentFile(fileId);
                fileUrls.Add(_formatter.Format(image.relativePath));
            }

            return ReplaceIdWithUrl(content, fileIds, fileUrls);
        }
    }
}
