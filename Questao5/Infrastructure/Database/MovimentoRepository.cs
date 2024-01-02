using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
	public class MovimentoRepository : DatabaseBootstrap, IMovimentoRepository
	{
		public MovimentoRepository(DatabaseConfig databaseConfig) : base(databaseConfig)
		{
		}

		public Task<double> GetSaldo(Guid idContaCorrente)
		{
			using var connection = new SqliteConnection(databaseConfig.Name);

			var movimentos = connection
				.Query(
					"SELECT tipomovimento, SUM(valor) as Total " +
					"FROM movimento " +
					"WHERE idcontacorrente = @idcontacorrente " +
					"GROUP BY tipomovimento " +
					"ORDER BY tipomovimento ",
					new
					{
						idcontacorrente = idContaCorrente.ToString().ToUpper(),
					});

			var SOMA_DOS_CREDITOS = movimentos
				.Where(m => m.tipomovimento == "C")
				.Select(t => t.Total)
				.FirstOrDefault(0);

			var SOMA_DOS_DEBITOS = movimentos
				.Where(m => m.tipomovimento == "D")
				.Select(t => t.Total)
				.FirstOrDefault(0);

			double Saldo = SOMA_DOS_CREDITOS - SOMA_DOS_DEBITOS;

			return Task.FromResult(Saldo);
		}

		public bool Insert(Movimento movimento)
		{
			var insertSql = @"INSERT INTO movimento (
							  IdMovimento, 
			                  IdContaCorrente, 
			                  DataMovimento, 
			                  TipoMovimento, 
			                  Valor)
			              VALUES(
			                  @IdMovimento, 
			                  @IdContaCorrente, 
			                  @DataMovimento, 
			                  @TipoMovimento, 
			                  @Valor)";

			using var connection = new SqliteConnection(databaseConfig.Name);
			var affectedRows = connection.Execute(insertSql, movimento);
			return affectedRows == 1;
		}

		IEnumerable<Movimento> IMovimentoRepository.GetAll()
		{
			using var connection = new SqliteConnection(databaseConfig.Name);

			var movimentos = connection
				.Query<Movimento>(
					"SELECT idmovimento, idcontacorrente, datamovimento, tipomovimento, valor " +
					"FROM movimento ");

			if (movimentos != null)
				return movimentos;

			return new List<Movimento>();
		}
	}
}
