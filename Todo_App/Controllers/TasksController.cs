using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.Tasks.Commands;
using Todo_App.Application.Tasks.Queries;

namespace Todo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ApiController
    {
        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateTaskCommand req) =>
            ResponseToFE(await Mediator.Send(req));

        [HttpPost("change-status")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeTaskStatusCommand req) =>
          ResponseToFE(await Mediator.Send(req));

        [HttpPost("delete-task/{taskId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(Guid taskId) =>
            ResponseToFE(await Mediator.Send(new DeleteTaskCommand
            {
                TaskItemId = taskId
            }));

        [HttpPost("edit-task")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand req)
        {
            return ResponseToFE(await Mediator.Send(req));
        }



        [HttpGet("my-tasks")]
        [Authorize(Roles = ("Admin, User"))]
        public async Task<IActionResult> GetAll() =>
            Ok(await Mediator.Send(new GetMyTasksQuery()));

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTasksByUserId(Guid userId)
        {
            return Ok(await Mediator.Send(new GetTasksByUserIdQuery
            {
                AssignedUserId = userId
            }));
        }
    }
}
