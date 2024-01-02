using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database
{
	public interface IMovimentoRepository
	{
		bool Insert(Movimento movimento);
		IEnumerable<Movimento> GetAll();
		Task<double> GetSaldo(Guid idContaCorrente);
	}
}
