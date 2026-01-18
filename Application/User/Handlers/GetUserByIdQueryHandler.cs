using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Todo_App.Application.User.Queries;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Users.Handlers
{
    public class GetUserByIdQueryHandler
        : IRequestHandler<GetUserByIdQuery, GUBI_Response>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserByIdQueryHandler(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GUBI_Response> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User
                ?? throw new UnauthorizedAccessException();

            var currentUserId = Guid.Parse(
                currentUser.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var isAdmin = currentUser.IsInRole("Admin");

            //User can access only himself
            if (!isAdmin && request.UserId != currentUserId)
                throw new UnauthorizedAccessException(
                    "You are not allowed to view this user");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            return new GUBI_Response
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "NoRole"
            };
        }
    }
}
