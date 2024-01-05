using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database
{
	public interface IIdempotenciaRepository
	{
		Idempotencia GetById(string id);
		IEnumerable<Idempotencia> GetAll();
		bool Insert(Idempotencia idempotencia);
		bool Update(string chave_Idempotencia, string resultado); 

	}
}
