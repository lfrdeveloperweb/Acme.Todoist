using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Security;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Resources;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Domain.Security;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Acme.Todoist.Application.Settings;
using MediatR;
using Acme.Todoist.Domain.Events.Accounts;

namespace Acme.Todoist.Application.Features.Accounts
{
    public static class ForgotPassword
    {
        public record Command(string SocialSecurityNumber, OperationContext Context) : Command<CommandResult>(Context);

        internal sealed class CommandHandler : CommandHandler<Command>
        {
            private readonly IPublisher _bus;
            private readonly ISecurityService _securityService;
            private readonly IKeyGenerator _keyGenerator;
            private readonly ISystemClock _systemClock;
            private readonly AccountSettings _accountSettings;

            public CommandHandler(
                ILoggerFactory loggerFactory,
                IUnitOfWork unitOfWork,
                IPublisher bus,
                ISecurityService securityService,
                IKeyGenerator keyGenerator,
                ISystemClock systemClock,
                IOptionsSnapshot<AccountSettings> accountSettings) : base(loggerFactory, unitOfWork)
            {
                _bus = bus;
                _securityService = securityService;
                _keyGenerator = keyGenerator;
                _systemClock = systemClock;
                _accountSettings = accountSettings.Value;
            }

            protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.UserRepository.GetByDocumentNumberAsync(command.SocialSecurityNumber, cancellationToken);
                if (user is null) return CommandResult.NotFound();

                var userToken = new UserToken<UserResetPasswordTokenData>(
                    userId: user.Id,
                    value: _keyGenerator.Generate(),
                    expiresAt: _systemClock.UtcNow.AddMinutes(_accountSettings.PasswordResetTokenExpirationInMinutes),
                    type: UserTokenType.ResetPasswordToken,
                    data: user.Email);

                await UnitOfWork.UserTokenRepository.CreateAsync(userToken);

                await _bus.Publish(new ForgotPasswordEvent(command.SocialSecurityNumber, userToken.Value));

                return CommandResult.NoContent();
            }
        }

        internal sealed class CommandValidator : CommandValidator<Command>
        {
            public CommandValidator(IUnitOfWork unitOfWork)
            {
                RuleFor(command => command.SocialSecurityNumber)
                    .IsValidEmail()
                    .MustAsync(unitOfWork.UserRepository.ExistByEmailAsync)
                    .WithMessageFromErrorCode(ReportCodeType.InvalidEmail);
            }
        }
    }
}
