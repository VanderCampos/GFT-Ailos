using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;

namespace Questao5.Infrastructure.Services.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MovimentoController : BaseController
	{
		public MovimentoController(IMediator mediator) : base(mediator)			
		{
		}

		[HttpPost]
		[ProducesResponseType(200, Type = typeof(Guid))]
		[ProducesResponseType(400, Type = typeof(string))]
		public async Task<IActionResult> Create(CreateMovimentacaoRequest command)
		{
			var result = await _mediator.Send(command);

			if (result.Data != null)
				return Ok(result.Data);

			return BadRequest(result.Errors);
		}

		[ProducesResponseType(200, Type = typeof(GetSaldoContaCorrenteResponse))]
		[ProducesResponseType(400, Type = typeof(string))]
		[HttpGet("GetSaldo/{idContaCorrente:guid}")]
		public async Task<ActionResult<GetSaldoContaCorrenteResponse>> GetSaldo(Guid idContaCorrente)
		{
			var result = await _mediator.Send(
				new GetSaldoContaCorrenteQuery()
				{
					idContaCorrente = idContaCorrente
				});

			if (result.Data != null)
				return Ok(result.Data);

			return BadRequest(result.Errors);
		}
	}
}