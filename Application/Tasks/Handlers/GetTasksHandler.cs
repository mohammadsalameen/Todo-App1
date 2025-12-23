using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Tasks.Commands;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tasks.Handlers
{
    public class GetTasksHandler : IRequestHandler<GetTasksQuery, List<TaskItem>>
    {
        private readonly AppDbContext _context;
        public GetTasksHandler(AppDbContext context) {
            _context = context;
        }
        public async Task<List<TaskItem>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            return await _context.Tasks.ToListAsync(cancellationToken);
        }
    }
}
