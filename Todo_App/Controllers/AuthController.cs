using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.Auth.Commands;
using Todo_App.Application.Auth.Queries;

namespace Todo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand req) =>
            ResponseToFE(await Mediator.Send(req));

        [HttpGet("")]
        public async Task<IActionResult> GetAll() =>
            Ok(await Mediator.Send(new GetUserQuery()));
    }
}
