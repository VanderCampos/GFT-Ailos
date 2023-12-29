using System;

namespace Questao1
{
	class ContaBancaria
	{
		private const double TAXASAQUE = 3.5;
		private readonly int numero;
		private string titular;
		//private double depositoInicial;
		private double saldo;

		public double Numero { get => Numero; }
		public string Titular { get => titular; set => titular = value; }
		//public double DepositoInicial { get => depositoInicial;	}
		public double Saldo { get => saldo; }

		public ContaBancaria(int numero, string titular)
		{
			this.numero = numero;
			Titular = titular;
		}

		public ContaBancaria(int numero, string titular, double depositoInicial)
		{
			this.numero = numero;
			this.titular = titular;
			//this.depositoInicial = depositoInicial;
			this.saldo = depositoInicial;
		}

		private void ValidaQuantia(double quantia)
		{
			if (quantia < 0)
				throw new Exception("Valor inválido!");
		}

		internal void Deposito(double quantia)
		{
			ValidaQuantia(quantia);
			saldo += quantia;
		}

		internal void Saque(double quantia)
		{
			ValidaQuantia(quantia);
			saldo -= quantia + TAXASAQUE;
		}

		public override string ToString()
		{
			return string.Format("Conta {0}, Titular: {1}, Saldo: $ {2,2:N2}", numero, titular, saldo);
		}
	}
}
