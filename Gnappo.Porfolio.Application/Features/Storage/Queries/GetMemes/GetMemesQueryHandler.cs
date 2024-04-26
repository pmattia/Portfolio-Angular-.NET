using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Helpers.UrlFormatters;
using Gnappo.Portfolio.Application.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Storage.Queries.GetMemes
{
    public class GetMemesQueryHandler : IRequestHandler<GetMemesQuery, ContentUrlDto[]>
    {
        private readonly IBlobService _blobService;
        private readonly IUrlFormatter _storageUrlFormatter;
        private readonly string _memesFolderName;

        public GetMemesQueryHandler(IBlobService blobService, IOptions<WebClientSettings> webClientSettings)
        {
            _blobService = blobService;
            _memesFolderName = webClientSettings.Value.MemesFolderName;

            _storageUrlFormatter = new StorageUrlFormatter(
                webClientSettings.Value.UrlPatterns.FileBinary
                );
        }

        public async Task<ContentUrlDto[]> Handle(GetMemesQuery request, CancellationToken cancellationToken)
        {
            var files = await _blobService.GetFileListAsync(_memesFolderName, cancellationToken);
            var contentUrls = files.Select(f => new ContentUrlDto()
            {
                Url = _storageUrlFormatter.Format($"{_memesFolderName}/{f.Name}"),
                Name = f.Name,
                Id = f.Name,
                ContentType = new ContentTypeInterpreter(f.Name).GetContentType()
            }).ToArray();
            return contentUrls;
        }
    }
}
