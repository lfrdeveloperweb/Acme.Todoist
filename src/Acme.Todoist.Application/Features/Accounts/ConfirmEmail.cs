using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Security;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Features.Accounts
{
    public static class ConfirmEmail
    {
        public record Command(
            string Email,
            string Token,
            OperationContext Context) : Command<CommandResult>(Context);

        internal sealed class CommandHandler : CommandHandler<Command>
        {
            private readonly ISystemClock _systemClock;

            public CommandHandler(
                ILoggerFactory loggerFactory,
                IUnitOfWork unitOfWork,
                ICommandValidator<Command> validator,
                ISystemClock systemClock) : base(loggerFactory, unitOfWork, validator)
            {
                _systemClock = systemClock;
            }

            protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.UserRepository.GetByEmailAsync(command.Email, cancellationToken);
                if (user is null) return CommandResult.NotFound();
                
                user.ConfirmEmail(_systemClock.UtcNow);
                user.UpdatedBy = Membership.From(user);

                await UnitOfWork.UserRepository.GetAsync<UserEmailConfirmationTokenData>(user.Id, UserTokenType.EmailConfirmationToken, command.Token, cancellationToken);

                await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

                return CommandResult.NoContent();
            }
        }

        internal sealed class CommandValidator : CommandValidator<Command>
        {
            public CommandValidator(IUnitOfWork unitOfWork)
            {
                RuleFor(command => command.Email)
                    .NotNullOrEmpty();

                RuleFor(command => command.Token)
                    .NotNullOrEmpty();
            }
        }
    }
}
