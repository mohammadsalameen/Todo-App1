using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common;
using Todo_App.Application.User.Commands;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Users.Handlers
{
    public class DeleteUserHandler
        : IRequestHandler<DeleteUserCommand, AbstractViewModel>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteUserHandler(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AbstractViewModel> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();

            try
            {
                var currentUser = _httpContextAccessor.HttpContext?.User;
                if (currentUser == null || !currentUser.IsInRole("Admin"))
                    throw new UnauthorizedAccessException("Admin only");

                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                    throw new Exception("User not found");

                //  Remove roles FIRST
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    var roleResult = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (!roleResult.Succeeded)
                        throw new Exception("Failed to remove user roles");
                }

                var tasks = await _context.TaskItems
                    .Where(t => t.AssignedUserId == user.Id || t.UserId == user.Id)
                    .Include(t => t.Comments)
                    .ToListAsync(cancellationToken);

                var taskComments = tasks.SelectMany(t => t.Comments).ToList();
                if (taskComments.Any())
                    _context.Comments.RemoveRange(taskComments);

               
                if (tasks.Any())
                    _context.TaskItems.RemoveRange(tasks);



                await _context.SaveChangesAsync(cancellationToken);

      
                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                    throw new Exception(
                        string.Join(", ", deleteResult.Errors.Select(e => e.Description))
                    );

                response.Success = true;
                response.Id = user.Id;
                response.Message = "User, roles, tasks, and comments deleted successfully";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.InnerException?.Message ?? ex.Message;
                return response;
            }
        }
    }

    }
