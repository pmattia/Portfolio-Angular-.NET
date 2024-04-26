using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Storage.Queries.GetBinary
{
    public class GetBinaryQuery : IRequest<FileBinaryDto>
    {
        public string filPath { get; set; }
    }
}
