using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Auth.Commands
{
    public class ConfirmEmailCommand : IRequest<AbstractViewModel>
    {
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}
