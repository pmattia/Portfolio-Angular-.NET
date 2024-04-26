using Gnappo.Portfolio.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Contracts.Infrastructure
{
    public interface ICognitiveService
    {
        public Task<string> SendMessageAsync(string message, CancellationToken cancellationToken);
        public Task<AiResponse> GetLastResponseAsync(CancellationToken cancellationToken);
    }
}
