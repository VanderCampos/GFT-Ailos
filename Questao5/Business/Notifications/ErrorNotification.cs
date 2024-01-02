using MediatR;

namespace Questao5.Business.Notifications
{
	public class ErrorNotification : INotification
	{
		public string ErrorCode { get; private set; }
		public string ErrorMessage { get; private set; }

		public ErrorNotification(string errorCode, string errorMessage)
		{
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}
	}
}
