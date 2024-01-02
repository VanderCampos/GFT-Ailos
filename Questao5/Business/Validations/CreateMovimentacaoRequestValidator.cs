using FluentValidation;
using MediatR;
using Questao5.Application.Commands.Requests;

namespace Questao5.Application.Business.Validations
{
	public class CreateMovimentacaoRequestValidator :BaseValidator<CreateMovimentacaoRequest>
	{
		public CreateMovimentacaoRequestValidator(IMediator mediator) : base(mediator)
		{
			RuleFor(command => command.TipoMovimento)
				.Must(x => x.Equals("C") || x.Equals("D"))
				.WithErrorCode("INVALID_TYPE")
				.WithMessage("Apenas os tipos “débito” ou “crédito” podem ser aceitos.");

			RuleFor(command => command.Valor)
				.GreaterThan(0)
				.WithErrorCode("INVALID_VALUE")
				.WithMessage("Apenas valores positivos podem ser recebidos.");
		}
	}
}
