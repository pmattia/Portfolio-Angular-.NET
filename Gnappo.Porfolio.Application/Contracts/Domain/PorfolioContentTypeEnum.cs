using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Gnappo.Portfolio.Application.Contracts.Domain
{
    public enum PortfolioContentTypeEnum
    {
        [Description("ContentUrl")]
        ContentUrl,
        [Description("Article")]    
        Article,
        [Description("Blog")]
        Blog,
        [Description("Contact")]
        Contact,
        [Description("Funny")]
        Funny,
        [Description("Pdf")]
        Pdf,
    }
}
