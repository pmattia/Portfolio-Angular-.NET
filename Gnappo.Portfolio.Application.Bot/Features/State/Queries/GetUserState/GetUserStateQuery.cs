using Gnappo.Portfolio.Application.Bot.Models;
using MediatR;
using Microsoft.Bot.Builder;

namespace Gnappo.Portfolio.Bot.Features.State.Queries.GetUserState
{
    public class GetUserStateQuery : IRequest<UserData>
    {
        public ITurnContext Context { get; set; }
    }
}
