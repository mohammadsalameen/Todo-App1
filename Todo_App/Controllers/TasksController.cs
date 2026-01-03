using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.Tasks.Commands;

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

        [HttpGet()]
        public async Task<IActionResult> GetAll() =>
            Ok(await Mediator.Send(new GetTasksQuery()));
    }
}
