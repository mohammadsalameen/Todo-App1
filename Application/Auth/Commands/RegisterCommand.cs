using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.Auth.Commands
{
    public class RegisterCommand : AuthCommandBase, IRequest<AbstractViewModel>
    {
        public string UserName { get; set; } = null!;
        public string Role { get; set; } = null!;
    }

}
