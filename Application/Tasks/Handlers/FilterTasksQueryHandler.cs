using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo_App.Application.Tasks.Queries;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

public class FilterTasksQueryHandler
    : IRequestHandler<FilterTasksQuery, List<GTPQ_Response>>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FilterTasksQueryHandler(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<GTPQ_Response>> Handle(
        FilterTasksQuery request,
        CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var isAdmin = user.IsInRole("Admin");

        var currentUserId = Guid.Parse(
            user.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        IQueryable<TaskItem> query = _context.TaskItems;

        
        if (isAdmin)
        {
           
            if (request.UserId.HasValue)
            {
                query = query.Where(t => t.AssignedUserId == request.UserId.Value);
            }
        }
        else
        {
           
            query = query.Where(t => t.AssignedUserId == currentUserId);
        }

      
        switch (request.Status.ToLower())
        {
            case "completed":
                query = query.Where(t => t.IsCompleted);
                break;

            case "urgent":
                query = query.Where(t => t.IsUrgent);
                break;

            case "all":
            default:
                break;
        }

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new GTPQ_Response
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                IsUrgent = t.IsUrgent
            })
            .ToListAsync(cancellationToken);
    }
}
