using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Storage.Queries.GetBinary
{
    internal class GetBinaryQueryHandler : IRequestHandler<GetBinaryQuery, FileBinaryDto>
    {
        private readonly IBlobService _blobService;

        public GetBinaryQueryHandler(IBlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task<FileBinaryDto> Handle(GetBinaryQuery request, CancellationToken cancellationToken)
        {
            var binary = await _blobService.GetFileBynaryAsync(request.filPath, cancellationToken);    
            return new FileBinaryDto()
            {
                Binary = binary,
                ContentType = new ContentTypeInterpreter(request.filPath).GetContentType()
            };
        }
    }
}
