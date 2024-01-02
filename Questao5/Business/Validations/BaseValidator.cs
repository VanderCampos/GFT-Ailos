using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Questao5.Business.Notifications;

namespace Questao5.Application.Business.Validations
{	
	public abstract class BaseValidator<T> : AbstractValidator<T>, INotification
	{
		public readonly IMediator _mediator;
		public readonly List<ErrorNotification> Error = new();

		public BaseValidator(IMediator mediator)
		{
			_mediator = mediator;
		}

		public string ErrorMessage()
		{
			var result = String.Empty;

			foreach (var erro in Error)
			{
				result += $"{erro.ErrorCode} - {erro.ErrorMessage}" + Environment.NewLine;				
			}

			return result;
		}

		public override ValidationResult Validate(ValidationContext<T> context)
		{
			var result = base.Validate(context);

			foreach (var erro in result.Errors)
			{
				//_mediator.Publish(new ErrorNotification($"Erro: {erro.ErrorCode} - {erro.ErrorMessage}"));		
				Error.Add(new ErrorNotification(erro.ErrorCode, erro.ErrorMessage));
			}		

			return result;
		}
	}
}
