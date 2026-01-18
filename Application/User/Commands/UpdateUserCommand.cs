using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.User.Commands
{
    public class UpdateUserCommand: IRequest<UUC_Response>
    {
        public Guid TargetUserId { get; set; }  // user being edited
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? NewPassword { get; set; }
        public string? Role { get; set; }
    }
    public class UUC_Response: AbstractViewModel
    {
        public string? UpdatedBy { get; set; } = null!;
    }
}
