using Acme.Todoist.Core.Repositories;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Infrastructure.Commands;
using Acme.Todoist.Infrastructure.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Core.Features.Commands;

public static class CloneTodo
{
    public record Command(string TodoId, OperationContext Context) : Command<CommandResult<Todo>>(Context);

    public sealed class CommandHandler : CommandHandler<Command, CommandResult<Todo>, IUnitOfWork>
    {
        private readonly ISender _dispatcher;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ISender dispatcher) : base(loggerFactory, unitOfWork)
        {
            _dispatcher = dispatcher;
        }

        protected override async Task<CommandResult<Todo>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var todo = await UnitOfWork.TodoRepository.GetByIdAsync(command.TodoId, cancellationToken);
            if (todo is null)
            {
                return CommandResult.NotFound<CommandResult<Todo>>();
            }

            var createTodoCommand = new CreateTodo.Command(
                todo.Title,
                todo.Description,
                todo.Project?.Id,
                todo.DueDate,
                todo.Priority,
                todo.Labels,
                command.OperationContext,
                BypassValidation: true);

            return await _dispatcher.Send(createTodoCommand, cancellationToken);
        }
    }
}