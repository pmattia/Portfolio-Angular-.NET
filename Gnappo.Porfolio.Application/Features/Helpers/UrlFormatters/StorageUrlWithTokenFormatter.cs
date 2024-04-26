using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters
{
    public class StorageUrlWithTokenFormatter : IUrlFormatter
    {
        private readonly string _fileUrlPattern;
        private readonly string _containerSasToken;

        public StorageUrlWithTokenFormatter(string fileUrlPattern, string containerSasToken)
        {
            _fileUrlPattern = fileUrlPattern;
            _containerSasToken = containerSasToken;
        }

        public string Format(string id)
        {
            return string.Format(_fileUrlPattern, id, _containerSasToken);
        }
    }
}
