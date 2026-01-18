using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Common;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.Auth.Queries
{
    public class GetUserQuery : IRequest<List<GUQ_Response>>
    {
    }

    public class GUQ_Response
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        //public Guid? UserId { get; set; }
        public UserRole Role { get; set; }
        //public List<TaskItem>? Tasks = new();
        //public List<Comment>? Comments = new();
    }
}
