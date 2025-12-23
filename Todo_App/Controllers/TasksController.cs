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
        public async Task<IActionResult> Create([FromBody] CreateTaskCommand req) =>
            ResponseToFE(await Mediator.Send(req));

        [HttpGet()]
        public async Task<IActionResult> GetAll([FromBody] int? userId) =>
            Ok(await Mediator.Send(new GetTasksQuery
            {
                UserId = (int)userId
            }));
    }
}
