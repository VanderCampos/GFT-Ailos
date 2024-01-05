using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Business.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Infrastructure.Services.Controllers
{
	/// <summary>
	/// Movimentar a conta corrente
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class MovimentoController : BaseController
	{
		public MovimentoController(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)			
		{
		}

		[SwaggerOperation(Summary = "Cria um movimento para a conta corrente.")]
		[SwaggerResponse(
			StatusCodes.Status200OK,
			"Retorna o Id do movimento gerado",
			typeof(Guid),
			"text/plain")]
		[SwaggerResponse(
			StatusCodes.Status400BadRequest,
			"Retorna uma mensagem descritiva de qual foi a falha e o tipo de falha.",
			typeof(string),
			"text/plain")]
		[HttpPost("{requestId:guid}")]
		public async Task<IActionResult> Create(Guid requestId, 
			[FromBody]CreateMovimentacaoRequest command)
		{			
			CommandResult? result = await CreateRequest(requestId, command);

			if (result != null)
				return CustomResponse(result);

			result = await _mediator.Send(command);

			UpdateRequest(requestId, result);

			return CustomResponse(result);
		}

		[SwaggerOperation(Summary = "Exibe o saldo atual da conta corrente")]
		[SwaggerResponse(
			StatusCodes.Status200OK,
			"Exibe número, nome, data e hora da consulta e o saldo atual da conta corrente.",
			typeof(GetSaldoContaCorrenteResponse),
			"application/json")]
		[SwaggerResponse(
			StatusCodes.Status400BadRequest,
			"Retorna uma mensagem descritiva de qual foi a falha e o tipo de falha.",
			typeof(string),
			"text/plain")]		
		[HttpGet("GetSaldo/{idContaCorrente:guid}")]
		public async Task<ActionResult<GetSaldoContaCorrenteResponse>> GetSaldo(Guid idContaCorrente)
		{
			var result = await _mediator.Send(
				new GetSaldoContaCorrenteQuery()
				{
					idContaCorrente = idContaCorrente
				});

			return CustomResponse(result);
		}
	}
}