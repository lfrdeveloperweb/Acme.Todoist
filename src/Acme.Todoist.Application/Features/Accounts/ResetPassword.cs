using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Cryptography;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Resources;
using Acme.Todoist.Domain.Security;
using FluentValidation;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Features.Accounts;

public static class ResetPassword
{
    public record Command(
        string Email,
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword,
        string Token,
        OperationContext Context) : Command<CommandResult>(Context);

    public sealed class CommandHandler : CommandHandler<Command>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISystemClock _systemClock;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<Command> validator,
            IPasswordHasher passwordHasher,
            ISystemClock systemClock) : base(loggerFactory, unitOfWork, validator)
        {
            _passwordHasher = passwordHasher;
            _systemClock = systemClock;
        }

        protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var user = await UnitOfWork.UserRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (user is null) return CommandResult.NotFound();

            if (!_passwordHasher.VerifyHashedPassword(user.PasswordHash, command.CurrentPassword))
            {
                return CommandResult.UnprocessableEntity(Report.Create(ReportCodeType.PasswordMismatch));
            }

            //var confirmPasswordResetResult = await _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword);
            //if (!confirmPasswordResetResult.Succeeded)
            //{
            //    return CommandResult.UnprocessableEntity<CommandResult>(ErrorsFrom(confirmPasswordResetResult));
            //}

            user.UpdatedBy = Membership.From(command.Context.Identity);
            user.UpdatedAt = _systemClock.UtcNow;

            Logger.LogWarning("Reset password applied to user {UserId} with email {Email}.", user.Id, user.Email);

            return CommandResult.NoContent();
        }

        public class CommandValidator : CommandValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(command => command.Email)
                    .NotNullOrEmpty()
                    .IsValidEmail();

                RuleFor(command => command.Token)
                    .NotNullOrEmpty();

                RuleFor(command => command.CurrentPassword)
                    .NotNullOrEmpty();

                RuleFor(request => request.NewPassword)
                    .NotNullOrEmpty()
                    .Password();

                RuleFor(request => request.ConfirmNewPassword)
                    .NotNullOrEmpty()
                    .Equal(request => request.NewPassword)
                    .WithMessageFromErrorCode(ReportCodeType.ConfirmPasswordNotMatch);
            }
        }
    }
}