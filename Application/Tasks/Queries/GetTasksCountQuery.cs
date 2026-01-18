using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Application.Tasks.Queries
{
    public class GetTasksCountQuery: IRequest<GTC_Response>
    {
        public Guid? UserId { get; set; }
    }
    public class GTC_Response
    {
        public int? CreatedTasks { get; set; }
        public int? UrgentTasks { get; set; }
        public int? CompletedTasks { get; set; }
    }
}
