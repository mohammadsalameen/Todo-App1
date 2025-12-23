using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tasks.Commands
{
    public class GetTasksQuery: IRequest<List<TaskItem>>
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; } 
        public int UserId { get; set; }
    }
}
