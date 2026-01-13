using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Todo_App.Application.User.Commands;
using Todo_App.Application.User.Queries;

namespace Todo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ApiController
    {
        [HttpPost("create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand req) =>
            ResponseToFE(await Mediator.Send(req));

        [HttpPost("update-user/{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserCommand req)
        {
            req.TargetUserId = userId;
            return ResponseToFE(await Mediator.Send(req));
        }

        [HttpPost("delete-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            return ResponseToFE(await Mediator.Send(new DeleteUserCommand { UserId = userId }));
        }
        [HttpGet()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers() =>
            Ok(await Mediator.Send(new GetAllUsersQuery()));

        [HttpGet("users-tasks")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserTasks()
        {
            var result = await Mediator.Send(new GetUserTasksQuery());
            return Ok(result);
        }
    }
}
