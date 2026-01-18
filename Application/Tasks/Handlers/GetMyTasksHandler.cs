using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Todo_App.Application.Tasks.Commands;
using Todo_App.Application.Tasks.Queries;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tasks.Handlers
{
    public class GetMyTasksHandler : IRequestHandler<GetMyTasksQuery, List<GMTQ_Response>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetMyTasksHandler(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<List<GMTQ_Response>> Handle(
            GetMyTasksQuery request,
            CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User
                ?? throw new UnauthorizedAccessException("User not authenticated");

            var userId = Guid.Parse(
                user.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var isAdmin = user.IsInRole("Admin");

            var query = _context.TaskItems.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(t => t.AssignedUserId == userId);
            }

            
            if (request.TaskId.HasValue)
            {
                query = query.Where(t => t.Id == request.TaskId.Value);
            }

            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<GMTQ_Response>>(tasks);
        }
    }
    }
