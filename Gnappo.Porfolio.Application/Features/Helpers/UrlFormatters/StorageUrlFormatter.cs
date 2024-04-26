using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters
{
    public class StorageUrlFormatter : IUrlFormatter
    {
        private readonly string _fileUrlPattern;

        public StorageUrlFormatter(string fileUrlPattern)
        {
            _fileUrlPattern = fileUrlPattern;
        }

        public string Format(string id)
        {
            return string.Format(_fileUrlPattern, id);
        }
    }
}
