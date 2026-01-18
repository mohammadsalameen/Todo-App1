using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Todo_App.Application.Common;
using Todo_App.Application.Tasks.Commands;
using Todo_App.DataAccess;

namespace Todo_App.Application.Tasks.Handlers
{
    public class ChangeTaskStatusHandler
        : IRequestHandler<ChangeTaskStatusCommand, AbstractViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangeTaskStatusHandler(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AbstractViewModel> Handle(
            ChangeTaskStatusCommand request,
            CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();

            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null)
                    throw new UnauthorizedAccessException();

                var userId = Guid.Parse(
                    user.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                var isAdmin = user.IsInRole("Admin");

                var task = await _context.TaskItems
                    .FirstOrDefaultAsync(t => t.Id == request.TaskItemId, cancellationToken);

                if (task == null)
                    throw new Exception("Task not found");

                if (!isAdmin && task.AssignedUserId != userId)
                    throw new UnauthorizedAccessException(
                        "You are not allowed to change this task");

                task.IsCompleted = request.IsCompleted;

                task.IsUrgent = request.IsUrgent;

                task.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);

                response.Success = true;
                response.Message = "Task status updated successfully";
                response.Id = task.Id;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
