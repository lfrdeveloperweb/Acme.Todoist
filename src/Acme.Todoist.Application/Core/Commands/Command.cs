using Acme.Todoist.Domain.Commons;
using MediatR;

namespace Acme.Todoist.Application.Core.Commands
{
    public interface ICommand<out TCommandResult> : IRequest<TCommandResult>
        where TCommandResult : CommandResult { }

    public interface ICommand : ICommand<CommandResult> { }

    public record Command<TCommandResult>(OperationContext OperationContext, bool BypassValidation = false) : ICommand<TCommandResult>
        where TCommandResult : CommandResult;
}
