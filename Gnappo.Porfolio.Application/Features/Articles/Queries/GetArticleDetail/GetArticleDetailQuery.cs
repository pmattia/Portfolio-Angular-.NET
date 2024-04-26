using MediatR;
using System;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetArticleDetail
{
    public class GetArticleDetailQuery: IRequest<ArticleDetailDto>
    {
        public string Name { get; set; }
    }
}
