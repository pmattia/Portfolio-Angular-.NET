using Gnappo.Portfolio.Application.Features.Topics.Queries;
using MediatR;
using System;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetMainTopics
{
    public class GetTopicQuery: IRequest<TopicInfoDto>
    {
        public string Name { get; set; }
    }
}
