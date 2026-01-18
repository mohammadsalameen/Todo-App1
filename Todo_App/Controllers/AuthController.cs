using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Todo_App.Application.Auth.Commands;
using Todo_App.Application.Auth.Handlers;
using Todo_App.Application.Auth.Queries;
using Todo_App.Application.Common.SignalR;

namespace Todo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        private readonly IMediator _mediator;


        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

  
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand req) =>
            ResponseToFE(await _mediator.Send(req));

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand req) =>
            ResponseToFE(await _mediator.Send(req));

        [HttpGet("")]
        public async Task<IActionResult> GetAll() =>
            Ok(await Mediator.Send(new GetUserQuery()));

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string userId)
        {
            var result = await _mediator.Send(new ConfirmEmailCommand
            {
                Token = token,
                UserId = userId
            });

            return ResponseToFE(result);
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode([FromBody] SendCodeCommand req) =>
            ResponseToFE(await _mediator.Send(req));

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand req) =>
            ResponseToFE(await _mediator.Send(req));
    }
}
