using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.User.Commands
{
    public class DeleteUserCommand: IRequest<AbstractViewModel>
    {
        public Guid UserId { get; set; }
    }
}
