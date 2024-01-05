using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Commands.Requests
{
	public class CreateMovimentacaoRequest : IRequest<CommandResult>
	{
		[SwaggerSchema(Description = "Id da Conta Corrente")]
		public string IdContaCorrente { get; set; }
		[SwaggerSchema(Description = "Tipo do movimento. C => Crédito e D => Débito")]
		public string TipoMovimento { get; set; }
		[SwaggerSchema(Description = "Valor do movimento")]
		public double Valor { get; set; }
		
	}
}
