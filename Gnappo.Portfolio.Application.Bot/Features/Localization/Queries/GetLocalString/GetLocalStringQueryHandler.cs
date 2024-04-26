using Gnappo.Portfolio.Application.Bot.Resources;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Features.Localization.Queries.GetLocalString
{
    public class GetLocalStringQueryHandler : IRequestHandler<GetLocalStringQuery, string>
    {
        private readonly IStringLocalizer _localizer;

        public GetLocalStringQueryHandler(IStringLocalizer<ConversationResources> localizer)
        {
            _localizer = localizer;
        }

        public async Task<string> Handle(GetLocalStringQuery request, CancellationToken cancellationToken)
        {
            return _localizer.GetString(request.Key);
        }
    }
}
