using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters
{
    internal class ContentTypeInterpreter
    {
        private readonly string _filename;
        public ContentTypeInterpreter(string filename)
        {
                _filename = filename;
        }
        public string GetContentType()
        {
            var extension = Path.GetExtension(_filename);
            return extension.ToLower() switch
            {
                ".svg" => "image/svg+xml",
                ".jpg" => MediaTypeNames.Image.Jpeg,
                ".jpeg" => MediaTypeNames.Image.Jpeg,
                ".png" => MediaTypeNames.Image.Jpeg,
                ".gif" => MediaTypeNames.Image.Gif,
                ".tiff" => MediaTypeNames.Image.Tiff,
                ".pdf" => MediaTypeNames.Application.Pdf,
                _ => MediaTypeNames.Application.Octet,
            };
        }
    }
}
