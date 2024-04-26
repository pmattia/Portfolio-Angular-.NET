using MediatR;
using System;
using System.Collections.Generic;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetArticleDetail
{
    public class GetArticlesQuery: IRequest<ArticleDto[]>
    {
        public IEnumerable<string> Tags { get; set; }
    }
}
