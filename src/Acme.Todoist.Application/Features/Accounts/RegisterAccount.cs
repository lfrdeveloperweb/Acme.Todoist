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
using Microsoft.Extensions.Internal;

namespace Acme.Todoist.Application.Features.Accounts;

public static class RegisterAccount
{
    public sealed record Command(
        string DocumentNumber,
        string Name,
        DateTime? BirthDate,
        string Email,
        string PhoneNumber,
        string UserName,
        string Password,
        string ConfirmPassword,
        OperationContext Context) : Command<CommandResult<User>>(Context);

    internal sealed class CommandHandler : CommandHandler<Command, CommandResult<User>>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IKeyGenerator _keyGenerator;
        private readonly ISystemClock _systemClock;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<Command> validator,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IKeyGenerator keyGenerator,
            ISystemClock systemClock) : base(loggerFactory, unitOfWork, validator, mapper)
        {
            _passwordHasher = passwordHasher;
            _keyGenerator = keyGenerator;
            _systemClock = systemClock;
        }

        protected override async Task<CommandResult<User>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var user = Mapper.Map<User>(command);
            
            user.Id = _keyGenerator.Generate();
            user.Role = Role.User;
            user.ChangePassword(_passwordHasher.HashPassword(command.Password));
            user.CreatedAt = _systemClock.UtcNow;

            await UnitOfWork.UserRepository.CreateAsync(user, cancellationToken);

            return CommandResult.Created(user);
        }
    }

    internal sealed class CommandValidator : CommandValidator<Command>
    {
        public CommandValidator(IUnitOfWork unitOfWork, ISystemClock systemClock)
        {
            RuleFor(command => command.Name)
                .NotNullOrEmpty();

            RuleFor(command => command.DocumentNumber)
                .NotNullOrEmpty()
                .IsValidCpf()
                .MustAsync(async (documentNumber, cancellationToken) => !await unitOfWork.UserRepository.ExistByDocumentNumberAsync(documentNumber, cancellationToken))
                .WithMessageFromErrorCode(ReportCodeType.DuplicatedDocumentNumber);

            RuleFor(command => command.BirthDate)
                .Must(birthDate => birthDate <= systemClock.UtcNow.Date)
                .When(command => command.BirthDate >= DateTime.MinValue);
            
            RuleFor(command => command.Email)
                .IsValidEmail()
                .MustAsync(async (email, cancellationToken) => !await unitOfWork.UserRepository.ExistByEmailAsync(email, cancellationToken))
                .WithMessageFromErrorCode(ReportCodeType.DuplicatedEmail);

            RuleFor(command => command.PhoneNumber)
                .IsValidPhoneNumber()
                .MustAsync(async (phoneNumber, cancellationToken) => !await unitOfWork.UserRepository.ExistByPhoneNumberAsync(phoneNumber, cancellationToken))
                .WithMessageFromErrorCode(ReportCodeType.DuplicatedPhoneNumber);
            
            RuleFor(command => command.UserName)
                .NotNullOrEmpty()
                .MustAsync(async (userName, cancellationToken) => !await unitOfWork.UserRepository.ExistByUserNameAsync(userName, cancellationToken))
                .WithMessageFromErrorCode(ReportCodeType.DuplicatedUserName);

            RuleFor(request => request.Password)
                .NotNullOrEmpty()
                .Password();

            RuleFor(request => request.ConfirmPassword)
                .NotNullOrEmpty()
                .Equal(request => request.Password).WithMessageFromErrorCode(ReportCodeType.ConfirmPasswordNotMatch);
        }
    }
}