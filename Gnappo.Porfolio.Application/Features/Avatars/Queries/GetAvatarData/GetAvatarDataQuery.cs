using Gnappo.Portfolio.Application.Contracts.Domain;
using MediatR;
using System.Collections.Generic;

namespace Gnappo.Portfolio.Application.Features.Avatars.Queries.GetAvatarChannelData
{
    public class GetAvatarDataQuery: IRequest<Dictionary<string, string>>
    {
        public AvatarEmotion Emotion { get; set; }
    }
}
