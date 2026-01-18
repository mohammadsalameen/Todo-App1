using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tasks.Commands
{
    public class GetMyTasksQuery : IRequest<List<GMTQ_Response>>
    {
        public Guid? TaskId { get; set; }
    }
    public class GMTQ_Response
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsUrgent { get; set; }
        public Guid UserId { get; set; }
    }
}
