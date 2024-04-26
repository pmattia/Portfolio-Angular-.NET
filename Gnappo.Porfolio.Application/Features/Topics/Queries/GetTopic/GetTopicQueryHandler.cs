using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Gnappo.Portfolio.Application.Features.Topics.Queries;
using System.Linq;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetMainTopics
{
    public class GetTopicQueryHandler : IRequestHandler<GetTopicQuery, TopicInfoDto>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public GetTopicQueryHandler(IMapper mapper, IBlobService blobService)
        {
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<TopicInfoDto> Handle(GetTopicQuery request, CancellationToken cancellationToken)
        {
            var topics = await _blobService.GetMainTopicsAsync(cancellationToken);
            var topic = topics.FirstOrDefault(a => a.Title == request.Name);
            return _mapper.Map<TopicInfoDto>(topic);
        }
    }
}
