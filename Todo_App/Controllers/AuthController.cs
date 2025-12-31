using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.Auth.Commands;
using Todo_App.Application.Auth.Handlers;
using Todo_App.Application.Auth.Queries;

namespace Todo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand req) =>
            ResponseToFE(await Mediator.Send(req));

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand req) =>
            ResponseToFE(await Mediator.Send(req));

        [HttpGet("")]
        public async Task<IActionResult> GetAll() =>
            Ok(await Mediator.Send(new GetUserQuery()));

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(
    [FromQuery] string token,
    [FromQuery] string userId)
        {
            var result = await Mediator.Send(new ConfirmEmailCommand
            {
                Token = token,
                UserId = userId
            });

            return ResponseToFE(result);
        }
    }
}
