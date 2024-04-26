using Gnappo.Portfolio.Application.Features.Topics.Queries;
using MediatR;
using System;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetMainTopics
{
    public class GetMainTopicsQuery: IRequest<TopicInfoDto[]>
    {
    }
}
