using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Todo_App.Application.Comments.Commands;
using Todo_App.Application.Common;
using Todo_App.DataAccess;

namespace Todo_App.Application.Comments.Handlers
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, AbstractViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteCommentHandler(AppDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AbstractViewModel> Handle(
            DeleteCommentCommand request,
            CancellationToken cancellationToken)
        {
            var response = new AbstractViewModel();

            try
            {
                var user = _httpContextAccessor.HttpContext?.User
                    ?? throw new UnauthorizedAccessException();

                var isAdmin = user.IsInRole("Admin");
                var currentUserId = Guid.Parse(
                    user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
                );

                var comment = await _context.Comments
                    .FirstOrDefaultAsync(c =>
                        c.Id == request.CommentId &&
                        c.TaskItemId == request.TaskItemId,
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
                        "You are not allowed to delete this comment");
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync(cancellationToken);

                response.Success = true;
                response.Message = "Comment deleted successfully";
                response.Id = comment.Id;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.LstErros?.Add(ex.Message);
                return response;
            }
        }

    }
}
