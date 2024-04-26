using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Avatars.Queries.GetAvatarChannelData
{
    public class GetAvatarDataQueryHandler : IRequestHandler<GetAvatarDataQuery, Dictionary<string, string>>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public GetAvatarDataQueryHandler(IMapper mapper, IBlobService blobService)
        {
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<Dictionary<string, string>> Handle(GetAvatarDataQuery request, CancellationToken cancellationToken)
        {
            return new Dictionary<string, string>
            {
                ["avatar"] = request.Emotion.ToString(),
                ["avatarUrl"] = _blobService.GetAvatarUrl(request.Emotion)
            };
        }
    }
}
