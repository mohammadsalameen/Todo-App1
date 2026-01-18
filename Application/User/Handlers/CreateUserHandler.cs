using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Todo_App.Application.Common;
using Todo_App.Application.User.Commands;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Users.Handlers
{
    public class CrreateUserhandler
        : IRequestHandler<CreateUserCommand, CUC_Response>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public CrreateUserhandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CUC_Response> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var response = new CUC_Response();
            try
            {
                // 1. Check email
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                    throw new Exception("Email already exists");

                // 2. Validate role
                if (!await _roleManager.RoleExistsAsync(request.Role))
                    throw new Exception("Role does not exist");

                // 3. Create user
                var user = new ApplicationUser
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    EmailConfirmed = true // Admin-created users
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    throw new Exception(string.Join(", ",
                        result.Errors.Select(e => e.Description)));

                // 4. Assign role
                await _userManager.AddToRoleAsync(user, request.Role);
                response.Success = true;
                response.Message = "User Created Successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.LstErros?.Add(ex.Message);
                return response;
            }
        }
    }
}
