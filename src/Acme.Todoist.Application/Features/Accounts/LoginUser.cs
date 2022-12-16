using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Application.Services;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Security;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Application.Features.Accounts;

public static class LoginUser
{
    public sealed record Command(
        string Email,
        string Password,
        OperationContext Context) : Command<CommandResult<JwtToken>>(Context);

    public sealed class CommandHandler : CommandHandler<Command, CommandResult<JwtToken>>
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<Command> validator,
            IJwtProvider jwtProvider,
            IDateTimeProvider dateTimeProvider) : base(loggerFactory, unitOfWork, validator)
        {
            _jwtProvider = jwtProvider;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override async Task<CommandResult<JwtToken>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var user = await UnitOfWork.UserRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (user is null)
            {
                return CommandResult.Unauthorized<CommandResult<JwtToken>>();
            }

            var jwtToken = _jwtProvider.Generate(user);

            user.ResetAccessFailedCount();
            user.IncreaseAccessCount(_dateTimeProvider.UtcNow);

            await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

            return CommandResult.Ok(jwtToken);
        }
    }

    /// <summary>
    /// Validator to validate request information about <see cref="User"/>.
    /// </summary>
    public sealed class CommandValidator : CommandValidator<Command>
    {
        public CommandValidator()
        {
            SetupValidation();
        }

        private void SetupValidation()
        {
            RuleFor(command => command.Email)
                .NotNullOrEmpty();

            RuleFor(command => command.Password)
                .NotNullOrEmpty();
        }
    }
}