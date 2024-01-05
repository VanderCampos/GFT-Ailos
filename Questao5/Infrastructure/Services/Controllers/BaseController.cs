using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;
using Questao5.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace Questao5.Infrastructure.Services.Controllers
{
	[ApiController]
	[Consumes("application/json")]	
	[Produces("application/json","text/plain")]	
	public abstract class BaseController : ControllerBase
	{
		protected readonly IMediator _mediator;
		protected readonly IRequestManager _requestManager;

		public BaseController(IMediator mediator, IRequestManager requestManager)
		{
			_mediator = mediator;
			_requestManager = requestManager;
		}

		protected async Task<CommandResult?> CreateRequest(Guid resquestId, object command)
		{
			var idempotencia = await _requestManager.Create(resquestId, command);

			if (String.IsNullOrEmpty(idempotencia.Resultado))
				return null;

			var commandResult = JsonSerializer.Deserialize<CommandResult>(idempotencia.Resultado);

			return commandResult;
		}

		protected void UpdateRequest(Guid resquestId, object command)
		{
			Task.FromResult(_requestManager.Update(resquestId, command));
		}

		protected ActionResult CustomResponse(CommandResult result)
		{
			if (!String.IsNullOrEmpty(result.Errors))
				return BadRequest((string)result.Errors);

			return Ok(result.Data);
		}
	}
}