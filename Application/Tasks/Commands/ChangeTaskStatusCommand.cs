using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Tasks.Commands
{
    public class ChangeTaskStatusCommand: IRequest<AbstractViewModel>
    {
        public Guid TaskItemId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsUrgent { get; set; }
    }
}
