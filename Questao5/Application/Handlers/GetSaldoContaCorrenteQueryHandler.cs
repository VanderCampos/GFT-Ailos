using MediatR;
using Questao5.Infrastructure.Database;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Application.Business.Validations;
using Questao5.Domain.Entities;
using Questao5.Application.Commands;

namespace Questao5.Application.Handlers
{
	public class GetSaldoContaCorrenteQueryHandler : IRequestHandler<GetSaldoContaCorrenteQuery,
		CommandResult>
	{
		private readonly IMovimentoRepository _repository;
		private readonly IContaCorrenteRepository _repositoryContaCorrente;
		private readonly IMediator _mediator;

		public GetSaldoContaCorrenteQueryHandler(IMediator mediator,
			IMovimentoRepository repository,
			IContaCorrenteRepository repositoryContaCorrente)
		{
			_mediator = mediator;
			_repository = repository;
			_repositoryContaCorrente = repositoryContaCorrente;
		}

		public Task<CommandResult> Handle(GetSaldoContaCorrenteQuery request,
					CancellationToken cancellationToken)
		{
			if (!Validate(request, out ContaCorrente contaCorrente, out string erros))
				return Task.FromResult(
					new CommandResult()
					{
						Errors = erros
					});

			var saldo = Task.FromResult(_repository.GetSaldo(request.idContaCorrente)).Result;

			return Task.FromResult(
					new CommandResult()
					{
						Data = new GetSaldoContaCorrenteResponse(
							numeroContaCorrente: contaCorrente.Numero,
							nomeTitularContaCorrente: contaCorrente.Nome,
							saldoAtual: saldo.Result)
					});
		}

		private bool Validate(GetSaldoContaCorrenteQuery request, out ContaCorrente contaCorrente, out string erros)
		{
			contaCorrente = _repositoryContaCorrente.GetById(request.idContaCorrente.ToString());

			var contaCorrenteValidator = new ContaCorrenteValidator(_mediator);
			var validationResultContaCorrente = contaCorrenteValidator.Validate(contaCorrente);

			erros = contaCorrenteValidator.ErrorMessage();

			return (validationResultContaCorrente.IsValid);
		}
	}
}
