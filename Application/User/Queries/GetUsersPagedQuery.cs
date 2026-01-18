using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common.Pagination;

namespace Todo_App.Application.User.Queries
{
    public class GetUsersPagedQuery: Pagination, IRequest<PagedResult<GUPQ_Response>>
    {
    }
    public class GUPQ_Response
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }
}
