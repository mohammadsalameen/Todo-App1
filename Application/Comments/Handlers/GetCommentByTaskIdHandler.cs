using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Comments.Commands;
using Todo_App.Application.Comments.Queries;
using Todo_App.Application.Tasks.Queries;
using Todo_App.DataAccess;

namespace Todo_App.Application.Comments.Handlers
{
    public class GetCommentByTaskIdHandler : IRequestHandler<GetCommentByTaskIdQuery, List<GCBTId_Response>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetCommentByTaskIdHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<GCBTId_Response>> Handle(GetCommentByTaskIdQuery request, CancellationToken cancellationToken)
        {
            var taskExists = await _context.TaskItems
                .AnyAsync(t => t.Id == request.TaskItemId, cancellationToken);

            if (!taskExists)
                throw new Exception("Task not found");

            
            var comments = await _context.Comments
                .Where(c => c.TaskItemId == request.TaskItemId)
                .Include(c => c.User)
                .Include(c => c.TaskItem)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync(cancellationToken);

            
            return _mapper.Map<List<GCBTId_Response>>(comments);
        }
    }
}
