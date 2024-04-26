using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Models
{
    public class WebClientSettings
    {
        public string WebPortfolioUrl { get; set; }
        public string MemesFolderName { get; set; }
        public string CvPdfFileId { get; set; }
        public string LinkedinUrl { get; set; }
        public UrlPatterns UrlPatterns { get; set; }
        public ContentTokens ContentTokens { get; set; }    
    }
    public class UrlPatterns
    {
        public string Article { get; set; }
        public string BlogPost { get; set; }
        public string Contact { get; set; }
        public string Pdf { get; set; }
        public string FileBinary { get; set; }
    }
    public class ContentTokens {         
        public string File { get; set; }
        public string Article { get; set; }
        public string BlogPost { get; set; }
        public string Pdf { get; set; }
    }   
}