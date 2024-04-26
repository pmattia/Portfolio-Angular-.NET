using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Application.Features.Contact.Commands.SendContactMessage
{
    public class SendContactMessageCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string? Reason { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}