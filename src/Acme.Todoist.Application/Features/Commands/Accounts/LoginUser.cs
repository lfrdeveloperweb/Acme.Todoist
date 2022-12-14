using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Application.Services;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.ValueObjects;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Domain.Commons;

namespace Acme.Todoist.Application.Features.Commands.Accounts;

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
            IMapper mapper,
            IDateTimeProvider dateTimeProvider) : base(loggerFactory, unitOfWork, validator, mapper: mapper)
        {
            _jwtProvider = jwtProvider;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override async Task<CommandResult<JwtToken>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var user = await UnitOfWork.UserRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (user is null)
            {

            }

            var jwtToken = _jwtProvider.Generate(user);

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