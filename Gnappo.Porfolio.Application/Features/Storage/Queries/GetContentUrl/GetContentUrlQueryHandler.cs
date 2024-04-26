using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using Gnappo.Portfolio.Application.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Storage.Queries.GetContentUrl
{
    internal class GetContentUrlQueryHandler : IRequestHandler<GetContentUrlQuery, ContentUrlDto>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IUrlFormatter _storageUrlFormatter;

        public GetContentUrlQueryHandler(IMapper mapper, IOptions<WebClientSettings> webClientSettings, IBlobService blobService)
        {
            _blobService = blobService;
            _mapper = mapper;
            _storageUrlFormatter = new StorageUrlFormatter(
                webClientSettings.Value.UrlPatterns.FileBinary
                );
        }

        public async Task<ContentUrlDto> Handle(GetContentUrlQuery request, CancellationToken cancellationToken)
        {
            var file = await _blobService.GetContentFileAsync(request.contentName, cancellationToken);
            var contentUrlDto = _mapper.Map<ContentUrlDto>(file);
            contentUrlDto.Url = _storageUrlFormatter.Format(file.relativePath);
            contentUrlDto.ContentType = new ContentTypeInterpreter(file.relativePath).GetContentType();
            return contentUrlDto;
        }
    }
}
