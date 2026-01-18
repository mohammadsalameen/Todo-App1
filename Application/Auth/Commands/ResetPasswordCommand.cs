using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<AbstractViewModel>
    {
        public string? NewPassword { get; set; }
        public string? Email { get; set; }
        public string? Code { get; set; }
    }
}
