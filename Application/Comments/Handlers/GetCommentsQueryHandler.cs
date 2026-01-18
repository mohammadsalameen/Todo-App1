using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Todo_App.Application.Auth.Queries;
using Todo_App.Application.Comments.Commands;
using Todo_App.Application.Common;
using Todo_App.DataAccess;

namespace Todo_App.Application.Comments.Handlers
{
    public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, List<GCQ_Response>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetCommentsQueryHandler(AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<List<GCQ_Response>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)){
                throw new UnauthorizedAccessException("User is not uthorized");
            }
            var userId = Guid.Parse(userIdString);
            var comments = await _context.Comments.Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ProjectTo<GCQ_Response>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            if (comments is null)
                throw new Exception("Comment not found");
            if (!comments.Any())
                return new List<GCQ_Response>();
            return _mapper.Map<List<GCQ_Response>>(comments);

        }
    }
}
