using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.Comments.Commands;
using Todo_App.Application.Comments.Queries;

namespace Todo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ApiController
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost()]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([FromBody] CreateCommentCommand req) =>
        ResponseToFE(await _mediator.Send(req));

        [HttpPost("delete-comment/{taskId}/{commentId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteComment(Guid taskId, Guid commentId) =>
            ResponseToFE(await _mediator.Send(new DeleteCommentCommand
            {
                TaskItemId = taskId,
                CommentId = commentId
            }));

        [HttpPost("update-comment")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentCommand req)
        {
            return ResponseToFE(await _mediator.Send(new UpdateCommentCommand
            {
                Id = req.Id,
                Content = req.Content,
            }));
        }

        [HttpGet("my-comments")]
        public async Task<IActionResult> GetAll() =>
        Ok(await _mediator.Send(new GetCommentsQuery()));

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskId(Guid taskId)
        {
            var result = await _mediator.Send(
                new GetCommentByTaskIdQuery { TaskItemId = taskId }
            );

            return Ok(result);
        }
    }
}
