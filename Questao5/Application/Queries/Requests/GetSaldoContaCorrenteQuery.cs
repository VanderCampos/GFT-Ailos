using MediatR;
using Questao5.Application.Commands;

namespace Questao5.Application.Queries.Requests
{
	public class GetSaldoContaCorrenteQuery : IRequest<CommandResult>
	{
		public Guid idContaCorrente { get; set; }
	}
}