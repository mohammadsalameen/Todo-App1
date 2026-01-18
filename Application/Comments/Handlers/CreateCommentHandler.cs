using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Todo_App.Application.Comments.Commands;
using Todo_App.Application.Common;
using Todo_App.Application.Common.SignalR;
using Todo_App.DataAccess;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Comments.Handlers
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, AbstractViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CreateCommentHandler(AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IHubContext<NotificationHub> hubContext
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        public async Task<AbstractViewModel> Handle(
            CreateCommentCommand request,
            CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();

            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Unauthorized";
                    return response;
                }

                var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var creatorName = user.FindFirstValue(ClaimTypes.Name);

                if (string.IsNullOrEmpty(userIdString) || string.IsNullOrEmpty(creatorName))
                {
                    response.Success = false;
                    response.Message = "Unauthorized";
                    return response;
                }

                var userId = Guid.Parse(userIdString);

                //  Get task with owner
                var task = await _context.TaskItems
                    .Include(t => t.User) // task owner
                    .FirstOrDefaultAsync(t => t.Id == request.TaskItemId, cancellationToken);

                if (task == null)
                {
                    response.Success = false;
                    response.Message = "Task does not exist";
                    return response;
                }

                // Create comment
                var comment = _mapper.Map<Comment>(request);
                comment.UserId = userId;
                comment.CreatedBy = creatorName;
                comment.TaskTitle = task.Title;

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync(cancellationToken);


                // Notify task owner (if not same user)
                var taskOwnerUserId = task.AssignedUserId.ToString();


                    await _hubContext.Clients
                        .User(taskOwnerUserId)
                        .SendAsync("OnCommentAdded", new
                        {
                            UserId = userIdString,
                            TaskId = task.Id,
                            TaskTitle = task.Title,
                            Comment = comment.Content,
                            CommentedBy = creatorName
                        });
              

                response.Success = true;
                response.Message = "Comment created successfully";
                response.Id = comment.Id;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

        }
    }
}
