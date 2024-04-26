using Gnappo.Portfolio.Application.Features.Contact.Commands.SendContactMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gnappo.Portfolio.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : Controller
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly IMediator _mediator;
        public ContactController(ILogger<ArticlesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost(Name = "Contact")]
        public async Task<ActionResult> Create([FromBody] SendContactMessageCommand sendContactMessageCommand)
        {
            var response = await _mediator.Send(sendContactMessageCommand);
            return Ok(response);
        }
    }
}
