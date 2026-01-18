using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Comments.Commands
{
    public class DeleteCommentCommand: IRequest<AbstractViewModel>
    {
        public Guid TaskItemId { get; set; }
        public Guid CommentId { get; set; }
    }
}
