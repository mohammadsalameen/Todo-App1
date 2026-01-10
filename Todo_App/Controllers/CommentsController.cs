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
        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCommentCommand req) =>
        ResponseToFE(await Mediator.Send(req));

        [HttpGet("my-comments")]
        public async Task<IActionResult> GetAll() =>
        Ok(await Mediator.Send(new GetCommentsQuery()));

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskId(Guid taskId)
        {
            var result = await Mediator.Send(
                new GetCommentByTaskIdQuery { TaskItemId = taskId }
            );

            return Ok(result);
        }
    }
}
