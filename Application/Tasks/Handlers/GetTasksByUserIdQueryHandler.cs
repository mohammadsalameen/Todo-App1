using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Todo_App.Application.Tasks.Queries;
using Todo_App.DataAccess;

namespace Todo_App.Application.Tasks.Handlers
{
    public class GetTasksByUserIdQueryHandler
        : IRequestHandler<GetTasksByUserIdQuery, List<GTBUQ_Response>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetTasksByUserIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GTBUQ_Response>> Handle(
            GetTasksByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.AssignedUserId, cancellationToken);

            if (!userExists)
                throw new Exception("User not found");

            
            var tasks = await _context.TaskItems
                .Where(t => t.AssignedUserId == request.AssignedUserId)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<GTBUQ_Response>>(tasks);
        }
    }
}
