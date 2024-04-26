using Gnappo.Portfolio.Application.Features.Storage.Queries.GetBinary;
using Gnappo.Portfolio.Application.Features.Storage.Queries.GetContentUrl;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gnappo.Portfolio.Api.Controllers
{
    [Route("file")]
    [ApiController]
    public class FileBinaryController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;
        private readonly IMediator _mediator;
        public FileBinaryController(ILogger<StorageController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpGet("{*filepath}")]
        public async Task<IActionResult> GetBinary(string filepath)
        {
            var getBinaryQuery = new GetBinaryQuery() { filPath=filepath };
            var fileDto = await _mediator.Send(getBinaryQuery);
            if (fileDto != null)
            {
                return File(fileDto.Binary, fileDto.ContentType);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
