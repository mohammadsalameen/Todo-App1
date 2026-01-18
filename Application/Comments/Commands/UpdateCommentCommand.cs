using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;
using Todo_App.Application.User.Commands;

namespace Todo_App.Application.Comments.Commands
{
    public class UpdateCommentCommand: IRequest<UUC_Response>
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
    }
    public class UCC_Response: AbstractViewModel
    {
        public string? UpdateBy { get; set; }
    }
}
