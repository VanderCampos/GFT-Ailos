using System.Runtime.CompilerServices;

namespace Questao5.Application.Commands
{
	public class CommandResult : ICommandResult
	{
		public string Errors { get; set; } = "";
		public object? Data { get; set; }

		public CommandResult()
		{
		}
	}
}
