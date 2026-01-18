using MediatR;
using System;

namespace Todo_App.Application.User.Queries
{
    public class GetUserByIdQuery : IRequest<GUBI_Response>
    {
        public Guid UserId { get; set; }
    }

    public class GUBI_Response
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
