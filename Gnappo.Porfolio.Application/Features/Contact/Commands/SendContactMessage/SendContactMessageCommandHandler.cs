using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Models.Email;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Application.Features.Contact.Commands.SendContactMessage
{
    public class SendContactMessageCommandHandler : IRequestHandler<SendContactMessageCommand, bool>
    {
        private readonly IEmailService _emailService;

        public SendContactMessageCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // ...

        public async Task<bool> Handle(SendContactMessageCommand request, CancellationToken cancellationToken)
        {
            var email = new EmailModel()
            {
                Subject = "Nuova richiesta da GNAPPO"
            };

            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("Richiesta da GNAPPO:<br />");

            bodyBuilder.Append($"Email: {request.Email}<br />");
            bodyBuilder.Append($"Nome: {request.Name}<br />");
            bodyBuilder.Append($"Motivazione: {request.Reason}<br />");
            bodyBuilder.Append($"Messaggio: {request.Message}");

            email.Body = bodyBuilder.ToString();

            return await _emailService.SendEmailAsync(email, cancellationToken);
        }
    }
}