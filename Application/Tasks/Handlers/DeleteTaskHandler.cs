using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Todo_App.Application.Common;
using Todo_App.Application.Tasks.Commands;
using Todo_App.DataAccess;

namespace Todo_App.Application.Tasks.Handlers
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, AbstractViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteTaskHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AbstractViewModel> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) throw new UnauthorizedAccessException();

                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var task = await _context.TaskItems
                    .Include(t => t.Comments)
                    .FirstOrDefaultAsync(t => t.Id == request.TaskItemId, cancellationToken);

                if (task == null)
                    throw new Exception("Task not found.");

                if (task.Comments.Any())
                    _context.Comments.RemoveRange(task.Comments);

                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync(cancellationToken);
                response.Success = true;
                response.Message = "Task deleted successfully.";
                response.Id = task.Id;
                return response;

            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
