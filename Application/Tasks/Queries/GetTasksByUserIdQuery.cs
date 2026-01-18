using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Application.Tasks.Queries
{
    public class GetTasksByUserIdQuery: IRequest<List<GTBUQ_Response>>
    {
        public Guid AssignedUserId { get; set; }
    }

    public class GTBUQ_Response
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsUrgent { get; set; }
        public Guid UserId { get; set; }

        
    }
}
