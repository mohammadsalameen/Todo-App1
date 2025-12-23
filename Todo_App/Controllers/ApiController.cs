using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.Common;

namespace Todo_App.Controllers
{
    public abstract class ApiController: ControllerBase
    {
        public ApiController()
        {
            Console.WriteLine("ApiController Constructor Called");
        }
        private IMediator _mediator;

        protected IActionResult ResponseToFE(AbstractViewModel res)
        {
            if (res == null) return StatusCode(500, "Response is null");
            return res.Success ? Ok(res) : BadRequest(res);
        }
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>(); 
    }
}
