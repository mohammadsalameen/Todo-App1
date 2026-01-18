using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.Application.User.Queries
{
    public class GetAllUsersQuery: IRequest<List<GAUQ_Response>>
    {
    }
    public class GAUQ_Response
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }
}
