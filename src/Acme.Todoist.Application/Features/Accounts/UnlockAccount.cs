﻿using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Application.Features.Accounts;

public static class UnlockAccount
{
    public sealed record Command(
        string UserId,
        OperationContext Context) : Command<CommandResult>(Context);

    internal sealed class CommandHandler : CommandHandler<Command>
    {
        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork) : base(loggerFactory, unitOfWork) { }

        protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user is null) return CommandResult.NotFound();

            if (!user.IsLocked) return CommandResult.NoContent();

            user.Unlock();

            await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

            return CommandResult.NoContent();
        }
    }
}