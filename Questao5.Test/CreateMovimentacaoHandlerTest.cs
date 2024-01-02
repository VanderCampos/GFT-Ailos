using MediatR;
using Moq;
using Questao5.Application.Business.Validations;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database;
using System.Net.Sockets;

namespace Questao5.Test.Application
{
	public class CreateMovimentacaoHandlerTest
	{
		readonly Mock<IMediator> _mediatorMock;
		readonly Mock<INotificationHandler<INotification>> _notificationHandlerMock;
		readonly List<INotification> _notifications;
		readonly Mock<IMovimentoRepository> _repositoryMovimentoMock;
		readonly Mock<IContaCorrenteRepository> _repositoryContaCorrenteMock;

		public CreateMovimentacaoHandlerTest()
		{
			_notifications = new List<INotification>();
			_mediatorMock = new Mock<IMediator>();
			_notificationHandlerMock = new Mock<INotificationHandler<INotification>>();
			_repositoryMovimentoMock = new Mock<IMovimentoRepository>();
			_repositoryContaCorrenteMock = new Mock<IContaCorrenteRepository>();

			_notificationHandlerMock.Setup(f => f.Handle(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
				.Callback((INotification n, CancellationToken c) =>
				{
					_notifications.Add(n);
				})
				.Returns(Task.CompletedTask);

			_mediatorMock.Setup(f => f.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
				.Callback((INotification n, CancellationToken c) =>
				{
					_notificationHandlerMock.Object.Handle(n, c);
				})
				.Returns(Task.CompletedTask);
		}

		[Fact]
		public void CreateMovimentacaoHandler_Test()
		{
			var request = new CreateMovimentacaoRequest()
			{
				IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
				TipoMovimento = "C",
				Valor = 10.2
			};

			var createMovimentacaoHandler = new CreateMovimentacaoHandler(_mediatorMock.Object,
				_repositoryMovimentoMock.Object,
				_repositoryContaCorrenteMock.Object);

			CancellationToken cancellationToken = new CancellationToken();

			ContaCorrente contaCorrente = new(request.IdContaCorrente, 123, "José da Silva", true);
			_repositoryContaCorrenteMock.Setup(m => m.GetById(request.IdContaCorrente))
				.Returns(contaCorrente);

			var result = createMovimentacaoHandler.Handle(request,cancellationToken).Result;

			Assert.NotNull(result.Data);
		}

		[Fact]
		public void CreateMovimentacaoHandler_INVALID_ACCOUNT_Test()
		{
			//Apenas contas correntes cadastradas podem receber movimentação;
			//TIPO: INVALID_ACCOUNT.

			var request = new CreateMovimentacaoRequest()
			{
				IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
				TipoMovimento = "CREDITO",
				Valor = 10.2
			};

			ContaCorrente contaCorrente = new(request.IdContaCorrente, 0, "José da Silva", true);
			_repositoryContaCorrenteMock.Setup(m => m.GetById(request.IdContaCorrente))
				.Returns(contaCorrente);

			var ContaCorrenteValidator = new ContaCorrenteValidator(_mediatorMock.Object);
			var contaCorrenteValidatorResult = ContaCorrenteValidator.Validate(contaCorrente);

			var erro = ContaCorrenteValidator.Error.Find(e => e.ErrorCode == "INVALID_ACCOUNT");

			Assert.NotNull(erro);

			Assert.True(ContaCorrenteValidator.Error.Count() == 1,
				"Múltiplos erros no request!");
			Assert.True(true);
		}

		[Fact]
		public void CreateMovimentacaoHandler_INACTIVE_ACCOUNT_Test()
		{
			//Apenas contas correntes ativas podem receber movimentação;
			//TIPO: INACTIVE_ACCOUNT.

			var request = new CreateMovimentacaoRequest()
			{
				IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
				TipoMovimento = "C",
				Valor = 10.2
			};

			ContaCorrente contaCorrente = new(request.IdContaCorrente,123,"José da Silva", false);
			_repositoryContaCorrenteMock.Setup(m => m.GetById(request.IdContaCorrente))
				.Returns(contaCorrente);

			var ContaCorrenteValidator = new ContaCorrenteValidator(_mediatorMock.Object);
			var contaCorrenteValidatorResult = ContaCorrenteValidator.Validate(contaCorrente);

			var erro = ContaCorrenteValidator.Error.Find(e => e.ErrorCode == "INACTIVE_ACCOUNT");

			Assert.NotNull(erro);

			Assert.True(ContaCorrenteValidator.Error.Count() == 1,
				"Múltiplos erros no request!");

		}

		[Fact]
		public void CreateMovimentacaoHandler_INVALID_VALUE_Test_Success()
		{
			//Apenas valores positivos podem ser recebidos;
			//TIPO: INVALID_VALUE.			

			var request = new CreateMovimentacaoRequest()
			{
				IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
				TipoMovimento = "C",
				Valor = 1
			};

			var createMovimentacaoRequestValidator = new CreateMovimentacaoRequestValidator(_mediatorMock.Object);
			var movimentacaoValidatorResult = createMovimentacaoRequestValidator.Validate(request);

			var erro = createMovimentacaoRequestValidator.Error.Find(e => e.ErrorCode == "INVALID_VALUE");

			Assert.Null(erro);

			Assert.True(createMovimentacaoRequestValidator.Error.Count() == 0,
				"Múltiplos erros no request!");
		}

		[Fact]
		public void CreateMovimentacaoHandler_INVALID_VALUE_Test_Error()
		{
			//Apenas valores positivos podem ser recebidos;
			//TIPO: INVALID_VALUE.	

			var request = new CreateMovimentacaoRequest()
			{
				IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
				TipoMovimento = "D",
				Valor = -1
			};

			var createMovimentacaoRequestValidator = new CreateMovimentacaoRequestValidator(_mediatorMock.Object);
			var movimentacaoValidatorResult = createMovimentacaoRequestValidator.Validate(request);

			var erro = createMovimentacaoRequestValidator.Error.Find(e => e.ErrorCode == "INVALID_VALUE");

			Assert.NotNull(erro);

			Assert.True(createMovimentacaoRequestValidator.Error.Count() == 1,
				"Múltiplos erros no request!");

		}

		[Fact]
		public void CreateMovimentacaoHandler_INVALID_TYPE_Test_Success()
		{
			//Apenas os tipos “débito” ou “crédito” podem ser aceitos;
			//TIPO: INVALID_TYPE.

			var request = new CreateMovimentacaoRequest()
			{
				IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
				TipoMovimento = "C",
				Valor = 1
			};

			var v = new CreateMovimentacaoRequestValidator(_mediatorMock.Object)
							.Validate(request);

			Assert.True(v.Errors.Count() == 0, "Existem erros no request!");
		}

		[Fact]
		public void CreateMovimentacaoHandler_INVALID_TYPE_Test_Error()
		{
			//Apenas os tipos “débito” ou “crédito” podem ser aceitos;
			//TIPO: INVALID_TYPE.

			var request = new CreateMovimentacaoRequest()
			{
				IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
				TipoMovimento = "CREDITO",
				Valor = 1
			};

			var createMovimentacaoRequestValidator = new CreateMovimentacaoRequestValidator(_mediatorMock.Object);
			var movimentacaoValidatorResult = createMovimentacaoRequestValidator.Validate(request);

			var erro = createMovimentacaoRequestValidator.Error.Find(e => e.ErrorCode == "INVALID_TYPE");

			Assert.NotNull(erro);

			Assert.True(createMovimentacaoRequestValidator.Error.Count() == 1,
				"Múltiplos erros no request!");

		}
	}
}

