using AutoMapper;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Articles.Queries.GetArticleDetail
{
    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, ArticleDto[]>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public GetArticlesQueryHandler(IMapper mapper, IBlobService blobService)
        {
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<ArticleDto[]> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            var articles = await _blobService.GetArticlesAsync(cancellationToken);
            var selectedArticles = articles.Where(a => request.Tags.Intersect(a.Tags).Count() > 0 && a.Published);
            return _mapper.Map<ArticleDto[]>(selectedArticles);
        }
    }
}
