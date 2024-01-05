using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Application.Queries.Responses
{
	public class GetSaldoContaCorrenteResponse
	{
		[SwaggerSchema(Description = "Número da conta corrente")]
		public int NumeroContaCorrente { get; private set; } = 0;
		[SwaggerSchema(Description = "Nome do titular da conta corrente")]
		public string NomeTitularContaCorrente { get; private set; } = String.Empty;
		[SwaggerSchema(Description = "Data e hora da resposta da consulta")]
		public DateTime DataHoraConsulta { get; private set; }
		[SwaggerSchema(Description = "Valor do Saldo atual")]
		public double SaldoAtual { get; private set; } = 0;

		public GetSaldoContaCorrenteResponse(int numeroContaCorrente,
			string nomeTitularContaCorrente, double saldoAtual)
		{
			NumeroContaCorrente = numeroContaCorrente;
			NomeTitularContaCorrente = nomeTitularContaCorrente;
			DataHoraConsulta = DateTime.Now;
			SaldoAtual = saldoAtual;
		}
	}
}
