using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Auth.Commands
{
    public class LoginCommand : AuthCommandBase, IRequest<LoginResponse>
    {
    }
    public class LoginResponse : AbstractViewModel
    {
        public string? AccessToken { get; set; }
    }
}