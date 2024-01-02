using MediatR;

namespace Questao5.Application.Commands.Requests
{
	public class CreateMovimentacaoRequest : IRequest<CommandResult>
	{
		public string IdContaCorrente { get; set; }
		public string TipoMovimento { get; set; }
		public double Valor { get; set; }
		
	}
}
