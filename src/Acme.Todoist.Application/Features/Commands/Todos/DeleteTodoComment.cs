﻿using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Infrastructure.Commands;
using Acme.Todoist.Infrastructure.Models;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Application.Features.Commands.Todos
{
    public static class DeleteTodoComment
    {
        public record Command(string TodoCommentId, string TodoId, OperationContext Context) : Command<CommandResult>(Context);

        public sealed class CommandHandler : CommandHandler<Command, IUnitOfWork>
        {
            public CommandHandler(
                ILoggerFactory loggerFactory,
                IUnitOfWork unitOfWork) : base(loggerFactory, unitOfWork) { }

            protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var comment = await UnitOfWork.TodoRepository.GetCommentByIdAsync(command.TodoCommentId, cancellationToken);
                if (comment is null || comment.TodoId != command.TodoId)
                {
                    return CommandResult.NotFound();
                }

                await UnitOfWork.TodoRepository.DeleteCommentAsync(comment, cancellationToken);

                return CommandResult.Ok();
            }
        }
    }
}