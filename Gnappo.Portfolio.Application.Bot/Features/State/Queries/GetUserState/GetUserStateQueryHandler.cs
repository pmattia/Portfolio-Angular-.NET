using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Application.Bot.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Features.State.Queries.GetUserState
{
    public class GetUserStateQueryHandler : IRequestHandler<GetUserStateQuery, UserData>
    {
        private readonly StateService _stateService;

        public GetUserStateQueryHandler(StateService stateService)
        {
            _stateService = stateService;
        }

        public async Task<UserData> Handle(GetUserStateQuery request, CancellationToken cancellationToken)
        {
            return await _stateService.UserProfileAccessor.GetAsync(request.Context, () => new UserData());
        }
    }
}