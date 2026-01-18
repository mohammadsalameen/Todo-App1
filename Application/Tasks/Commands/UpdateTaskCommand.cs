using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Tasks.Commands
{
    public class UpdateTaskCommand: IRequest<AbstractViewModel>
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsUrgent { get; set; }
    }
}
