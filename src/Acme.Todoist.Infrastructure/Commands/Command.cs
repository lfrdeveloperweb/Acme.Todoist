using Acme.Todoist.Infrastructure.Models;
using MediatR;

namespace Acme.Todoist.Infrastructure.Commands
{
    public interface ICommand<out TCommandResult> : IRequest<TCommandResult>
        where TCommandResult : CommandResult { }

    public interface ICommand : ICommand<CommandResult> { }

    public record Command<TCommandResult>(OperationContext OperationContext, bool BypassValidation = false) : ICommand<TCommandResult>
        where TCommandResult : CommandResult;
}
