using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Topics.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetMainTopics
{
    public class GetMainTopicsQueryHandler : IRequestHandler<GetMainTopicsQuery, TopicInfoDto[]>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public GetMainTopicsQueryHandler(IMapper mapper, IBlobService blobService)
        {
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<TopicInfoDto[]> Handle(GetMainTopicsQuery request, CancellationToken cancellationToken)
        {
            var articles = await _blobService.GetMainTopicsAsync(cancellationToken);
            return _mapper.Map<TopicInfoDto[]>(articles);
        }
    }
}
