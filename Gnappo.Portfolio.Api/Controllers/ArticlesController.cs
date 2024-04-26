
using Gnappo.Portfolio.Application.Features.Articles.Queries.GetArticleDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gnappo.Portfolio.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly IMediator _mediator;
        public ArticlesController(ILogger<ArticlesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{name}", Name = "GetArticleByName")]
        public async Task<ActionResult<ArticleDetailDto>> Details(string name)
        {
            var getArticleDetailQuery = new GetArticleDetailQuery() { Name = name };
            var article = await _mediator.Send(getArticleDetailQuery);
            if (article != null)
            {
                return Ok(article);
            }
            else
            {
                return NotFound();
            }
        }
    }
}