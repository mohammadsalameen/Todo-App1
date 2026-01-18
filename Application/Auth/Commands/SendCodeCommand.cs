using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Auth.Commands
{
    public class SendCodeCommand : IRequest<AbstractViewModel>
    {
        public string Email { get; set; }
    }
}
