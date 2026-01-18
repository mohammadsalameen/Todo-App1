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
    public class UpdateTaskHandler
        : IRequestHandler<UpdateTaskCommand, AbstractViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateTaskHandler(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AbstractViewModel> Handle(
            UpdateTaskCommand request,
            CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();

            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var userId = Guid.Parse(
                    user!.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                var isAdmin = user.IsInRole("Admin");

                var task = await _context.TaskItems
                    .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
                if ( !isAdmin && task.AssignedUserId != userId)
                    throw new UnauthorizedAccessException("Not allowed");
                if (task == null)
                {
                    response.Success = false;
                    response.LstErros?.Add("Task Not found");
                    return response;
                }
                    
                    // 🔹 Update task
                    task.Title = request.Title;
                    task.Description = request.Description;
                task.IsUrgent = request.IsUrgent;
                    task.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync(cancellationToken);

                response.Success = true;
                response.Id = task.Id;
                response.Message = "Task updated successfully";

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
