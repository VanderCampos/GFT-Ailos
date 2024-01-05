using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database;
using System.Text.Json;

namespace Questao5.Infrastructure.Services
{
	public class RequestManager : IRequestManager
	{
		private readonly IIdempotenciaRepository _idempotenciaRepository;

		public RequestManager(IIdempotenciaRepository idempotenciaRepository)
		{
			_idempotenciaRepository = idempotenciaRepository;
		}

		public Task<Idempotencia> Create(Guid requestId, object command)
		{
			var Chave_Idempotencia = requestId.ToString().ToUpperInvariant();
			var idempotencia = _idempotenciaRepository.GetById(Chave_Idempotencia);

			if (idempotencia.Chave_Idempotencia.Equals(Chave_Idempotencia))
				return Task.FromResult(idempotencia);

			idempotencia.Chave_Idempotencia = Chave_Idempotencia;
			idempotencia.Requisicao = JsonSerializer.Serialize(command);

			_idempotenciaRepository.Insert(idempotencia);

			return Task.FromResult(idempotencia);
		}

		public Task<bool> Update(Guid requestId, object commandResult)
		{
			var Chave_Idempotencia = requestId.ToString().ToUpperInvariant();
			var resultado = JsonSerializer.Serialize(commandResult);			
			_idempotenciaRepository.Update(Chave_Idempotencia, resultado);
			return Task.FromResult(true);
		}
	}
}
