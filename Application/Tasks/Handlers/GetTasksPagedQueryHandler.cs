using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Todo_App.Application.Common.Pagination;
using Todo_App.Application.Tasks.Queries;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tasks.Handlers
{
    public class GetTasksPagedQueryHandler : IRequestHandler<GetTasksPagedQuery, PagedResult<GTPQ_Response>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTasksPagedQueryHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PagedResult<GTPQ_Response>> Handle(
            GetTasksPagedQuery request,
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

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(t =>
                    t.Title.Contains(request.Search) ||
                    t.Description.Contains(request.Search)
                );
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

            var totalCount = await query.CountAsync(cancellationToken);

            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new GTPQ_Response
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    IsUrgent = t.IsUrgent
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<GTPQ_Response>
            {
                Items = tasks,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
    }
