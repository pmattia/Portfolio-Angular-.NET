using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Storage.Queries.GetBinary
{
    public class FileBinaryDto
    {
        public byte[] Binary { get; set; }
        public string ContentType { get; set; }
    }
}
