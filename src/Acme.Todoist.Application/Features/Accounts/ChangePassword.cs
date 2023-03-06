using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Cryptography;
using Acme.Todoist.Application.Core.Security;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Resources;
using Acme.Todoist.Domain.Security;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Features.Accounts
{
    internal static class ChangePassword
    {
        public record Command(
            string CurrentPassword,
            string NewPassword,
            string ConfirmNewPassword,
            OperationContext Context) : Command<CommandResult>(Context);

        internal sealed class CommandHandler : CommandHandler<Command>
        {
            private readonly ISecurityService _securityService;
            private readonly IPasswordHasher _passwordHasher;

            public CommandHandler(
                ILoggerFactory loggerFactory,
                IUnitOfWork unitOfWork,
                ICommandValidator<Command> validator,
                ISecurityService securityService,
                IPasswordHasher passwordHasher) : base(loggerFactory, unitOfWork, validator)
            {
                _securityService = securityService;
                _passwordHasher = passwordHasher;
            }

            protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.UserRepository.GetByIdAsync(command.Context.Identity.Id, cancellationToken);

                var signInResult = await _securityService.CheckPasswordAsync(user, command.CurrentPassword);
                if (!signInResult.Succeeded)
                {
                    return CommandResult.UnprocessableEntity(ReportCodeType.InvalidPassword);
                }

                user.ChangePassword(_passwordHasher.HashPassword(command.NewPassword));

                await UnitOfWork.UserRepository.ChangePasswordAsync(user, cancellationToken);

                return CommandResult.NoContent();
            }

            internal sealed class CommandValidator : CommandValidator<Command>
            {
                public CommandValidator()
                {
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
}
