using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common.Pagination;

namespace Todo_App.Application.Tasks.Queries
{
    public class GetTasksPagedQuery: Pagination, IRequest<PagedResult<GTPQ_Response>>
    {
        public Guid? UserId { get; set; }
    }

}
