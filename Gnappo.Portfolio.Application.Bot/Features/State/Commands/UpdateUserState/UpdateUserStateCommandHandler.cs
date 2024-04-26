
using Gnappo.Portfolio.Application.Bot.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Bot.Features.State.Commands.UpdateUserState
{
    public class UpdateUserStateCommandHandler : IRequestHandler<UpdateUserStateCommand>
    {
        private readonly StateService _stateService;

        public UpdateUserStateCommandHandler(StateService stateService)
        {
            _stateService = stateService;
        }

        public async Task<Unit> Handle(UpdateUserStateCommand request, CancellationToken cancellationToken)
        {
            await _stateService.UserProfileAccessor.SetAsync(request.Context, request.UserData);

            return Unit.Value;
        }
    }
}