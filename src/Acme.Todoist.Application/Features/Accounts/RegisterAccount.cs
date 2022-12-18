using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Application.Core.Cryptography;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Resources;
using Acme.Todoist.Domain.Security;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Acme.Todoist.Application.Features.Accounts;

public static class RegisterAccount
{
    public sealed record Command(
        string Name,
        DateTime? BirthDate,
        string Email,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,
        OperationContext Context) : Command<CommandResult<User>>(Context);

    public sealed class CommandHandler : CommandHandler<Command, CommandResult<User>>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IKeyGenerator _keyGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<Command> validator,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IKeyGenerator keyGenerator,
            IDateTimeProvider dateTimeProvider) : base(loggerFactory, unitOfWork, validator, mapper)
        {
            _passwordHasher = passwordHasher;
            _keyGenerator = keyGenerator;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override async Task<CommandResult<User>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var user = Mapper.Map<User>(command);
            
            user.Id = _keyGenerator.Generate();
            user.Role = Role.User;
            user.SetPassword(_passwordHasher.HashPassword(command.Password));
            user.CreatedAt = _dateTimeProvider.UtcNow;

            await UnitOfWork.UserRepository.CreateAsync(user, cancellationToken);

            return CommandResult.Created(user);
        }
    }

    /// <summary>
    /// Validator to validate request information about <see cref="User"/>.
    /// </summary>
    public sealed class CommandValidator : CommandValidator<Command>
    {
        public CommandValidator(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            RuleFor(command => command.Name)
                .NotNullOrEmpty();

            RuleFor(command => command.BirthDate)
                .Must(birthDate => birthDate <= dateTimeProvider.BrasiliaNow.Date)
                .When(command => command.BirthDate >= DateTime.MinValue);

            RuleFor(command => command.Email)
                .NotNullOrEmpty()
                .MaxLength(Email.MaxLength)
                .IsValidEmail()
                .MustAsync(async (email, cancellationToken) => !await unitOfWork.UserRepository.ExistByEmailAsync(email, cancellationToken))
                .WithMessageFromErrorCode(ReportCodeType.DuplicatedEmail); ;

            RuleFor(command => command.PhoneNumber)
                .NotNullOrEmpty()
                .MaxLength(PhoneNumber.MaxLength)
                .IsValidPhoneNumber()
                .MustAsync(async (phoneNumber, cancellationToken) => !await unitOfWork.UserRepository.ExistByPhoneNumberAsync(phoneNumber, cancellationToken))
                .WithMessageFromErrorCode(ReportCodeType.DuplicatedPhoneNumber);

            RuleFor(request => request.Password)
                .NotNullOrEmpty()
                .Password();

            RuleFor(request => request.ConfirmPassword)
                .NotNullOrEmpty()
                .Equal(request => request.Password).WithMessageFromErrorCode(ReportCodeType.ConfirmPasswordNotMatch);
        }
    }
}