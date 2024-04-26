using Gnappo.Portfolio.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailModel email, CancellationToken cancellationToken);
        bool SendEmail(EmailModel email);
    }
}
