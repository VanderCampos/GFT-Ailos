using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
	public class ContaCorrenteRepository : DatabaseBootstrap, IContaCorrenteRepository
	{
		public ContaCorrenteRepository(DatabaseConfig databaseConfig) : base(databaseConfig)
		{
		}

		public ContaCorrente GetById(string id)
		{
			using var connection = new SqliteConnection(databaseConfig.Name);

			var contaCorrente = connection
				.QueryFirstOrDefault<ContaCorrente>(
					"SELECT idcontacorrente, numero, nome, ativo " +
					"FROM [ContaCorrente] " +
					"WHERE [idcontacorrente]=@idcontacorrente",
					new
					{
						idcontacorrente = id.ToUpperInvariant(),
					});

			if (contaCorrente != null)
				return contaCorrente;

			return new ContaCorrente();
		}

		public IEnumerable<ContaCorrente> GetAll()
		{
			using var connection = new SqliteConnection(databaseConfig.Name);

			var contasCorrente = connection
				.Query<ContaCorrente>(
					"SELECT idcontacorrente, numero, nome, ativo " +
					"FROM [ContaCorrente] ");

			if (contasCorrente != null)
				return contasCorrente;

			return new List<ContaCorrente>();
		}
	}
}
