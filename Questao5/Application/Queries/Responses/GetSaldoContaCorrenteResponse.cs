namespace Questao5.Application.Queries.Responses
{
	public class GetSaldoContaCorrenteResponse
	{
		public int NumeroContaCorrente { get; private set; } = 0;
		public string NomeTitularContaCorrente { get; private set; } = String.Empty;
		public DateTime DataHoraConsulta { get; private set; }
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
