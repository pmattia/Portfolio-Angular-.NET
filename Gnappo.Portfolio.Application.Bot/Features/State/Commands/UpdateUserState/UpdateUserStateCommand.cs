
using Gnappo.Portfolio.Application.Bot.Models;
using MediatR;
using Microsoft.Bot.Builder;

namespace Gnappo.Portfolio.Bot.Features.State.Commands.UpdateUserState
{
    public class UpdateUserStateCommand : IRequest
    {
        public ITurnContext Context { get; set; }
        public UserData UserData;
    }
}