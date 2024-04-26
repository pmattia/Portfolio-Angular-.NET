using MediatR;

namespace Gnappo.Portfolio.Bot.Features.Localization.Queries.GetLocalString
{
    public class GetLocalStringQuery : IRequest<string>
    {
        public string Key { get; set; }
    }
}
