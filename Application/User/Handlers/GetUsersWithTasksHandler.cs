using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.User.Queries;
using Todo_App.DataAccess;

namespace Todo_App.Application.User.Handlers
{
    public class GetUsersWithTasksHandler : IRequestHandler<GetUserTasksQuery, List<UserWithTasksResponse>>
    {
        private readonly AppDbContext _context;

        public GetUsersWithTasksHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserWithTasksResponse>> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .Include(u => u.Tasks)
                .ThenInclude(t => t.Comments)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var result = users.Select(u => new UserWithTasksResponse
            {
                UserId = u.Id,
                UserName = u.UserName,
                Tasks = u.Tasks.Select(t => new TaskWithCommentsResponse
                {
                    TaskId = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    IsUrgent = t.IsUrgent,
                    Comments = t.Comments.Select(c => new CommentResponse
                    {
                        CommentId = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt
                    }).ToList()
                }).ToList()
                }).ToList();
            return result;
        }
    }
}
