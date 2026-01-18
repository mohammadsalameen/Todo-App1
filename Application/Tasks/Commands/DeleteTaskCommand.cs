using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Tasks.Commands
{
    public class DeleteTaskCommand: IRequest<AbstractViewModel>
    {
        public Guid TaskItemId { get; set; }
    }
}
