using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Application.Features.Commands.Todos;

public static class CloneTodo
{
    public record Command(string TodoId, OperationContext Context) : Command<CommandResult<Todo>>(Context);

    public sealed class CommandHandler : CommandHandler<Command, CommandResult<Todo>>
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