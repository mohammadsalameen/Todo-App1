using MediatR;
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
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand req) =>
            ResponseToFE(await _mediator.Send(req));

        [HttpPost("update-user/{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserCommand req)
        {
            req.TargetUserId = userId;
            return ResponseToFE(await _mediator.Send(req));
        }

        [HttpPost("delete-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            return ResponseToFE(await _mediator.Send(new DeleteUserCommand { UserId = userId }));
        }

        [HttpGet()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers() =>
            Ok(await _mediator.Send(new GetAllUsersQuery()));

        [HttpGet("users-tasks")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserTasks()
        {
            var result = await _mediator.Send(new GetUserTasksQuery());
            return Ok(result);
        }
                [HttpGet("paged")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult>GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _mediator.Send(new GetUsersPagedQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search
            });
            return Ok(result);
        }
        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            return Ok(
                await _mediator.Send(new GetUserByIdQuery { UserId = userId })
            );
        }
    }
}
