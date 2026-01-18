using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Comments.Commands
{
    public class CreateCommentCommand: IRequest<AbstractViewModel>
    {
        public string Content { get; set; } = null!;
        public string? TaskTitle { get; set; }
        public Guid TaskItemId {get; set; }
    }
}
