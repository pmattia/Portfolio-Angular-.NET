using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Models.Email
{
    public class EmailSettings
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string ConnectionString { get; set; }
    }
}
