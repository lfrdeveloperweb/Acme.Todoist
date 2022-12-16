using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Security;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Features.Accounts;

public static class LockAccount
{
    public sealed record Command(
        string UserId,
        OperationContext Context) : Command<CommandResult>(Context);

    public sealed class CommandHandler : CommandHandler<Command>
    {
        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork) : base(loggerFactory, unitOfWork) { }

        protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user is null)
            {
                return CommandResult.NotFound();
            }

            user.Lock();

            await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

            return CommandResult.Ok();
        }
    }
}