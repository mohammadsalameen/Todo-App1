using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Todo_App.Application.User.Commands;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Users.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UUC_Response>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IHttpContextAccessor _httpContext;

        public UpdateUserHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContext = httpContext;
        }

        public async Task<UUC_Response> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new UUC_Response();
            try
            {
                var currentUserId =Guid.Parse(_httpContext.HttpContext!.User
                             .FindFirstValue(ClaimTypes.NameIdentifier)!);

                var isAdmin = _httpContext.HttpContext.User.IsInRole("Admin");

                //  User editing someone else
                if (!isAdmin && currentUserId != request.TargetUserId)
                    throw new Exception("You are not allowed to edit this user");

                var user = await _userManager.FindByIdAsync(request.TargetUserId.ToString());
                if (user == null)
                    throw new Exception("User not found");

                //  Basic fields (User + Admin)
                user.UserName = request.UserName;
                user.Email = request.Email;
                user.EmailConfirmed = true;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    throw new Exception(string.Join(", ",
                        updateResult.Errors.Select(e => e.Description)));

                // Password (User + Admin)
                if (!string.IsNullOrWhiteSpace(request.NewPassword))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await _userManager.ResetPasswordAsync(
                        user, token, request.NewPassword);

                    if (!passwordResult.Succeeded)
                        throw new Exception(string.Join(", ",
                            passwordResult.Errors.Select(e => e.Description)));
                }

                //  Role (ADMIN ONLY)
                if (isAdmin && !string.IsNullOrWhiteSpace(request.Role))
                {
                    if (!await _roleManager.RoleExistsAsync(request.Role))
                        throw new Exception("Role does not exist");

                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, request.Role);
                }

                // User tried to change role
                if (!isAdmin && request.Role != null)
                    throw new Exception("You are not allowed to change role");

                response.Success = true;
                response.Message = "User updated successfully";
                return response;
            }catch(Exception ex)
            {
                response.Success = false;
                response.LstErros?.Add(ex.Message);
                return response;
            }
        }
    }
}
