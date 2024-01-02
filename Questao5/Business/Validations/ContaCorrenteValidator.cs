using FluentValidation;
using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Application.Business.Validations
{
	public class ContaCorrenteValidator : BaseValidator<ContaCorrente>
	{
		public ContaCorrenteValidator(IMediator mediator) : base(mediator)
		{
			RuleFor(command => command.Numero)
				.NotEmpty()
				.WithErrorCode("INVALID_ACCOUNT")
				.WithMessage("Apenas contas correntes cadastradas podem receber movimentação");

			RuleFor(command => command.Ativo)
				.Must(x => x.Equals(true))
				.WithErrorCode("INACTIVE_ACCOUNT")
				.WithMessage("Apenas contas correntes ativas podem receber movimentação");
		}
	}
}
