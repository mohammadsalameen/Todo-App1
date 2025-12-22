using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Todo_App.Controllers
{
    public abstract class ApiController: ControllerBase
    {
        public ApiController()
        {
            Console.WriteLine("ApiController Constructor Called");
        }
        private IMediator _mediator;

        public IActionResult ResponseToFE(AbstractViewModel res)
        {
            if (res.success) return Ok(res);
            else return BadRequest(res);
        }
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
