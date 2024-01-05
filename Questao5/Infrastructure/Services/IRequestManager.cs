using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Services
{
	public interface IRequestManager
	{		
		Task<Idempotencia> Create(Guid requestId, object command);
		Task<bool> Update(Guid requestId, object commandResult);
	}
}
