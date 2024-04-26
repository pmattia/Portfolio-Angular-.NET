using Gnappo.Portfolio.Application.Features.Articles.Queries.GetMainTopics;
using Gnappo.Portfolio.Application.Features.Topics.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gnappo.Portfolio.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicsController : Controller
    {
        private readonly ILogger<TopicsController> _logger;
        private readonly IMediator _mediator;
        public TopicsController(ILogger<TopicsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(Name = "GetTopics")]
        public async Task<ActionResult<TopicInfoDto[]>> GetAll()
        {
            var getMainArticlesQuery = new GetMainTopicsQuery();
            var topics = await _mediator.Send(getMainArticlesQuery);
            if (topics != null)
            {
                return Ok(topics);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
