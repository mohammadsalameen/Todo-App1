using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Auth.Queries;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Auth.Handlers
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, List<GUQ_Response>>
    {
        private readonly AppDbContext _context;
        public GetUserHandler(AppDbContext context)
        {
            _context = context;

        }
        public async Task<List<GUQ_Response>> Handle(GetUserQuery request,CancellationToken cancellationToken)
        {
            var user = await _context.Users.Select(u => new GUQ_Response
            {
                UserName = u.UserName,
                Email = u.Email
            }).ToListAsync(cancellationToken);

            if (user is null)
                throw new Exception("User not found");
            return user;
        }

    }
}