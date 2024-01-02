using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database
{
	public interface IContaCorrenteRepository
	{
		ContaCorrente GetById(string id);
		IEnumerable<ContaCorrente> GetAll();		
	}
}
