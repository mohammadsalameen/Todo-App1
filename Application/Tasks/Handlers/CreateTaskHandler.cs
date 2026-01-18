using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Todo_App.Application.Common;
using Todo_App.Application.Common.SignalR;
using Todo_App.Application.Tasks.Commands;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tasks.Handlers
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, AbstractViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CreateTaskHandler(AppDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IHubContext<NotificationHub> hubContext
            )
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }
        public async Task<AbstractViewModel> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();
            try
            {
                var userIdString = _httpContextAccessor.HttpContext?
                 .User.FindFirstValue(ClaimTypes.NameIdentifier);
                var creatorName = _httpContextAccessor.HttpContext?
                 .User.FindFirstValue(ClaimTypes.Name);
               

                if (string.IsNullOrEmpty(userIdString) || string.IsNullOrEmpty(creatorName))
                {
                    response.Success = false;
                    response.Message = "Unauthorized";
                    return response;
                }

                var creatorId = Guid.Parse(userIdString);
                var assignedUserExists = await _context.Users.AnyAsync(u => u.Id == creatorId);
                if (!assignedUserExists)
                {
                    response.Success = false;
                    response.Message = "Assigned user does not exist";
                    return response;
                }
                var task = _mapper.Map<TaskItem>(request);
                task.UserId = creatorId;
                task.AssignedUserId = request.AssignedUserId;
                task.CreatedBy = creatorName;
                task.AssignedUserName = request.AssignedUserName;
                response.Id = task.Id;

                _context.TaskItems.Add(task);
                await _context.SaveChangesAsync(cancellationToken);


                await _hubContext.Clients
                    .User(request.AssignedUserId.ToString())
                    .SendAsync("OnTaskAssigned", new
                    {
                        UserId = userIdString,
                        TaskId = task.Id,
                        Title = task.Title,
                        AssignedBy = creatorName
                    });


                if (!string.IsNullOrWhiteSpace(request.Comment))
                {
                    var comment = new Comment
                    {
                        Content = request.Comment,
                        TaskItemId = task.Id,
                        UserId = creatorId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Comments.Add(comment);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while creating the task.";
                response.LstErros = new List<string> { ex.Message };
                return response;
            }
        }
    }
}
