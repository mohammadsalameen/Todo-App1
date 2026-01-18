using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Auth.Queries;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Auth.Handlers
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, List<GUQ_Response>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetUserHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<GUQ_Response>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            //var user = await _context.Users.Select(u => new GUQ_Response
            //{
            //    UserName = u.UserName,
            //    Email = u.Email,
            //    UserId = u.Id,
            //}).ToListAsync(cancellationToken);

            var user = await _context.Users.ProjectTo<GUQ_Response>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (user is null)
                throw new Exception("User not found");
            return user;
        }

    }
}