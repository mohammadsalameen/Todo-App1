using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Todo_App.Application.Tasks.Queries;
using Todo_App.DataAccess;

namespace Todo_App.Application.Tasks.Handlers
{
    public class GetTasksCountHandler : IRequestHandler<GetTasksCountQuery, GTC_Response>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTasksCountHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GTC_Response> Handle(GetTasksCountQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;
            var isAdmin = user.IsInRole("Admin");
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var query = _context.TaskItems.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(t => t.AssignedUserId == userId);
            }
            return new GTC_Response
            {
                CreatedTasks = await query.CountAsync(cancellationToken),
                UrgentTasks = await query.CountAsync(t => t.IsUrgent, cancellationToken),
                CompletedTasks = await query.CountAsync(t => t.IsCompleted, cancellationToken)
            };
        }
    }
}
