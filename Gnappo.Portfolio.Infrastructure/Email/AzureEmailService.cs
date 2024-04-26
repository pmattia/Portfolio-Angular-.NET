using Azure;
using Azure.Communication.Email;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Features.Contact.Commands.SendContactMessage;
using Gnappo.Portfolio.Application.Models.Email;
using Gnappo.Portfolio.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gnappo.Portfolio.Infrastructure.Email
{
    public class AzureEmailService : IEmailService
    {
        public string _fromAddress { get; }
        public string _toAddress { get; }
        private readonly EmailClient _emailClient;
        private readonly ILogger<AzureEmailService> _logger;

        public AzureEmailService(IOptions<AzureEmailSettings> emailSettings, ILogger<AzureEmailService> logger)
        {
            _logger = logger;
            _fromAddress = emailSettings.Value.FromAddress;
            _toAddress = emailSettings.Value.ToAddress;
            _emailClient = new EmailClient(emailSettings.Value.ConnectionString);
        }

        public bool SendEmail(EmailModel email)
        {
            try
            {
                var emailSendOperation = _emailClient.Send(
                    wait: WaitUntil.Completed,
                    senderAddress: _fromAddress, // The email address of the domain registered with the Communication Services resource
                    recipientAddress: _toAddress,
                    subject: email.Subject,
                    htmlContent: $"<html><body>{email.Body}</body></html>");
                Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                string operationId = emailSendOperation.Id;
                Console.WriteLine($"Email operation id = {operationId}");

                return true;
            }
            catch (RequestFailedException ex)
            {
                /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                _logger.LogError($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");

                return false;
            }
        }


        public async Task<bool> SendEmailAsync(EmailModel email, CancellationToken cancellationToken)
        {
            try
            {
                var emailSendOperation = await _emailClient.SendAsync(
                    wait: WaitUntil.Completed,
                    senderAddress: _fromAddress, // The email address of the domain registered with the Communication Services resource
                    recipientAddress: _toAddress,
                    subject: email.Subject,
                    htmlContent: $"<html><body>{email.Body}</body></html>",
                    cancellationToken: cancellationToken);
                Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                string operationId = emailSendOperation.Id;
                Console.WriteLine($"Email operation id = {operationId}");

                return true;
            }
            catch (RequestFailedException ex)
            {
                /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                _logger.LogError($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");

                return false;
            }
        }
    }
}
