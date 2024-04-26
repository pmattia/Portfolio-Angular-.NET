using Gnappo.Portfolio.Application.Features.Articles.Queries.GetArticleDetail;
using Gnappo.Portfolio.Application.Features.Blog.Queries.GetBlogPostDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gnappo.Portfolio.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IMediator _mediator;
        public BlogController(ILogger<BlogController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{name}", Name = "GetBlogPostByName")]
        public async Task<ActionResult<BlogPostDetailDto>> Details(string name)
        {
            var getBlogPostDetailQuery = new GetBlogPostDetailQuery() { Name = name };
            var blogPost = await _mediator.Send(getBlogPostDetailQuery);
            if (blogPost != null)
            {
                return Ok(blogPost);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
