using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Auth.Queries
{
    public class GetUserQuery: IRequest<List<GUQ_Response>>
    {
        public int? Id { get; set; }
    }

    public class GUQ_Response
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        //public List<TaskItem>? Tasks = new() ;
        //public List<Comment>? Comments = new(); 
    }
}
