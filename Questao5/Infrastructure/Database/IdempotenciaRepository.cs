using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database
{
	public class IdempotenciaRepository : DatabaseBootstrap, IIdempotenciaRepository
	{
		public IdempotenciaRepository(DatabaseConfig databaseConfig) : base(databaseConfig)
		{
		}

		public IEnumerable<Idempotencia> GetAll()
		{
			using var connection = new SqliteConnection(databaseConfig.Name);

			var idempotencia = connection
				.Query<Idempotencia>(
					"SELECT Chave_Idempotencia, Requisicao, Resultado " +
					"FROM idempotencia ");

			if (idempotencia != null)
				return idempotencia;

			return new List<Idempotencia>();
		}

		Idempotencia IIdempotenciaRepository.GetById(string requestId)
		{
			using var connection = new SqliteConnection(databaseConfig.Name);

			var idempotencia = connection
				.QueryFirstOrDefault<Idempotencia>(
					"SELECT Chave_Idempotencia, Requisicao, Resultado " +
					"FROM idempotencia " +
					"WHERE Chave_Idempotencia=@Chave_Idempotencia",
					new
					{
						Chave_Idempotencia = requestId.ToUpperInvariant(),
					});

			if (idempotencia != null)
				return idempotencia;

			return new Idempotencia();
		}

		bool IIdempotenciaRepository.Insert(Idempotencia idempotencia)
		{
			var insertSql = @"INSERT INTO idempotencia (
							  Chave_Idempotencia, 
							  Requisicao, 
							  Resultado)
			              VALUES(
			                  @Chave_Idempotencia, 
							  @Requisicao, 
							  @Resultado)";

			using var connection = new SqliteConnection(databaseConfig.Name);
			var affectedRows = connection.Execute(insertSql, idempotencia);
			return affectedRows == 1;
		}

		bool IIdempotenciaRepository.Update(string chave_Idempotencia, string resultado)
		{
			var updateSql = @"Update idempotencia 
							  SET Resultado = @Resultado			              
							  WHERE Chave_Idempotencia=@Chave_Idempotencia";

			using var connection = new SqliteConnection(databaseConfig.Name);
			var updateParams = new
			{
				Chave_Idempotencia = chave_Idempotencia,
				Resultado = resultado
			};

			var affectedRows = connection.Execute(updateSql, updateParams);
			return affectedRows == 1;
		}
	}
}
