using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Comments.Commands;
using Todo_App.Application.Common;
using Todo_App.Application.User.Commands;
using Todo_App.DataAccess;

namespace Todo_App.Application.Comments.Handlers
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, UUC_Response>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateCommentCommandHandler(AppDbContext context, 
            IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UUC_Response> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var response = new UUC_Response();
            try
            {
                var user = _httpContextAccessor.HttpContext?.User
                    ?? throw new UnauthorizedAccessException();
                var isAdmin = user.IsInRole("Admin");
                var currentUserId = Guid.Parse(
                    user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
                );
                var currentUser = user.FindFirst(
                    System.Security.Claims.ClaimTypes.Name
                )?.Value;

                var comment = await _context.Comments
                    .FirstOrDefaultAsync(c =>
                        c.Id == request.Id,
                        cancellationToken);

                if (comment == null)
                {
                    response.Success = false;
                    response.LstErros?.Add("Comment not found");
                    return response;
                }
                if (!isAdmin && comment.UserId != currentUserId)
                {
                    throw new UnauthorizedAccessException(
                        "You are not allowed to update this comment");
                }
                comment.Content = request.Content;
                comment.UpdatedBy = currentUser;
                await _context.SaveChangesAsync(cancellationToken);
                response.Success = true;
                response.Message = "Comment Updated Successfully";
                response.UpdatedBy = currentUser; 
                return response;
            }catch(Exception ex)
            {
                response.Success = false;
                response.LstErros?.Add(ex.Message);
                return response;
            }
        }
    }
}
