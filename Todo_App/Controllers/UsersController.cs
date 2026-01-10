using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.User.Queries;

namespace Todo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ApiController
    {
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
