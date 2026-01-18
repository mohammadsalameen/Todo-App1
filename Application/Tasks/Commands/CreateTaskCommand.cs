using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;

namespace Todo_App.Application.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<AbstractViewModel>
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid AssignedUserId { get; set; }
        public string? AssignedUserName { get; set; }
        public string? Comment { get; set; }
        public bool IsUrgent { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
