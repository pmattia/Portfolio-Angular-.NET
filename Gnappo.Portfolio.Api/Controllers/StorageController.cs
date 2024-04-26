using Gnappo.Portfolio.Application.Features.Storage.Queries;
using Gnappo.Portfolio.Application.Features.Storage.Queries.GetContentUrl;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gnappo.Portfolio.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : Controller
    {
        private readonly ILogger<StorageController> _logger;
        private readonly IMediator _mediator;
        public StorageController(ILogger<StorageController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpGet("{name}", Name = "GetFileByName")]
        public async Task<ActionResult<ContentUrlDto>> Details(string name)
        {
            var getContentUrlQuery = new GetContentUrlQuery() { contentName = name};
            var contentUrl = await _mediator.Send(getContentUrlQuery);
            if (contentUrl != null)
            {
                return Ok(contentUrl);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
