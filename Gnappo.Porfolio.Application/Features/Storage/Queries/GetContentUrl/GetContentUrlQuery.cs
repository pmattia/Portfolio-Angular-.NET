using MediatR;

namespace Gnappo.Portfolio.Application.Features.Storage.Queries.GetContentUrl
{
    public class GetContentUrlQuery : IRequest<ContentUrlDto>
    {
        public string contentName { get; set; }
    }
}
