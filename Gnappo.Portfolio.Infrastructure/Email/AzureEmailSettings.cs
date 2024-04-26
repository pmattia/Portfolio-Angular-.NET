using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Infrastructure.Email
{
    public class AzureEmailSettings
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string ConnectionString { get; set; }
    }
}
