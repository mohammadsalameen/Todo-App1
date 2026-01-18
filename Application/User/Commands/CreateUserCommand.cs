using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.User.Commands
{
    public class CreateUserCommand: IRequest<CUC_Response>
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User";
    }
    public class CUC_Response: AbstractViewModel
    {
        public Guid userId { get; set; }
    }
}
