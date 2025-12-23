using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Auth.Commands;
using Todo_App.Application.Common;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.Auth.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, AbstractViewModel>
    {
        private readonly AppDbContext _context;
        public CreateUserHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AbstractViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = request.Password,
                Role = Enum.TryParse<UserRole>(request.Role, out var role) ? role : UserRole.User,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            response.Id = user.Id;
            response.Success = true;
            return response;

        }
    }
}
