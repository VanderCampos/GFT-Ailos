using MediatR;
using Questao5.Application.Business.Validations;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database;

namespace Questao5.Application.Handlers
{
	public class CreateMovimentacaoHandler : IRequestHandler<CreateMovimentacaoRequest, CommandResult>
	{
		private readonly IMediator _mediator;
		private readonly IMovimentoRepository _repository;
		private readonly IContaCorrenteRepository _repositoryContaCorrente;

		public CreateMovimentacaoHandler(IMediator mediator,
			IMovimentoRepository repository,
			IContaCorrenteRepository repositoryContaCorrente)
		{
			_repository = repository;
			_mediator = mediator;
			_repositoryContaCorrente = repositoryContaCorrente;
		}

		public Task<CommandResult> Handle(CreateMovimentacaoRequest command, CancellationToken cancellationToken)
		{
			if (!Validate(command, out string erros))
				return Task.FromResult(
					new CommandResult()
					{
						Errors = erros
					});

			var movimento = new Movimento()
			{
				IdMovimento = Guid.NewGuid().ToString().ToUpper(),
				DataMovimento = DateTime.Now,

				IdContaCorrente = command.IdContaCorrente,
				TipoMovimento = command.TipoMovimento,
				Valor = command.Valor				
			};

			_repository.Insert(movimento);
			
			return Task.FromResult(
					new CommandResult()
					{
						//Data = Guid.Parse(movimento.IdMovimento.ToUpper())
						Data = movimento.IdMovimento.ToUpper()
					});
		}

		private bool Validate(CreateMovimentacaoRequest request, out string erros)
		{		
			var ContaCorrente = _repositoryContaCorrente.GetById(request.IdContaCorrente);

			var contaCorrenteValidator = new ContaCorrenteValidator(_mediator);
			var validationResultContaCorrente = contaCorrenteValidator.Validate(ContaCorrente);

			var movimentacaoRequestValidator = new CreateMovimentacaoRequestValidator(_mediator);
			var validationResultMovimentacao = movimentacaoRequestValidator.Validate(request);

			validationResultMovimentacao.Errors.AddRange(validationResultContaCorrente.Errors);

			erros = contaCorrenteValidator.ErrorMessage()				
				+ movimentacaoRequestValidator.ErrorMessage();

			return ((validationResultContaCorrente.IsValid) && (validationResultMovimentacao.IsValid));
		}
	}
}
