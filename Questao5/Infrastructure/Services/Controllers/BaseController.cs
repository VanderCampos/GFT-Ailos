using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Questao5.Infrastructure.Services.Controllers
{
	[ApiController]
	public abstract class BaseController : ControllerBase
	{
		protected readonly IMediator _mediator;

		public BaseController(IMediator mediator)
        {
			_mediator = mediator;						
		}
	}
}