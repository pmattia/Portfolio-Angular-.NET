using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Storage.Queries.GetMemes
{
    public class GetMemesQuery : IRequest<ContentUrlDto[]>
    {
    }
}
