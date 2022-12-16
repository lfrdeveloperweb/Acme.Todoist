using System;
using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Core.Cryptography;
using Acme.Todoist.Domain.Security;

namespace Acme.Todoist.Application.Features.Accounts;

public static class RegisterAccount
{
    public sealed record Command(
        string Name,
        DateOnly? BirthDate,
        string Email,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,
        OperationContext Context) : Command<CommandResult<User>>(Context);

    public sealed class CommandHandler : CommandHandler<Command, CommandResult<User>>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<Command> validator,
            IPasswordHasher passwordHasher,
            IDateTimeProvider dateTimeProvider) : base(loggerFactory, unitOfWork, validator)
        {
            _passwordHasher = passwordHasher;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override async Task<CommandResult<User>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var user = await UnitOfWork.UserRepository.GetByEmailAsync(command.Email, cancellationToken);
            user.SetPassword(_passwordHasher.HashPassword(command.Password));

            user.CreatedBy = Membership.From(command.Context.Identity);
            user.CreatedAt = _dateTimeProvider.BrasiliaNow;

            await UnitOfWork.UserRepository.CreateAsync(user, cancellationToken);

            return CommandResult.Ok(user);
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
            RuleFor(command => command.Name)
                .NotNullOrEmpty();

            RuleFor(command => command.Email)
                .NotNullOrEmpty();

            RuleFor(command => command.Password)
                .NotNullOrEmpty();
        }
    }
}