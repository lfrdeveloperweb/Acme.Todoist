﻿using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Security;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Application.Services;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Resources;
using Acme.Todoist.Domain.Security;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Features.Accounts;

public static class LoginUser
{
    public sealed record Command(
        string Email,
        string Password,
        OperationContext Context) : Command<CommandResult<JwtToken>>(Context);

    internal sealed class CommandHandler : CommandHandler<Command, CommandResult<JwtToken>>
    {
        private readonly ISecurityService _securityService;
        private readonly IJwtProvider _jwtProvider;
        private readonly ISystemClock _systemClock;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<Command> validator,
            ISecurityService securityService,
            IJwtProvider jwtProvider,
            ISystemClock systemClock) : base(loggerFactory, unitOfWork, validator)
        {
            _securityService = securityService;
            _jwtProvider = jwtProvider;
            _systemClock = systemClock;
        }

        protected override async Task<CommandResult<JwtToken>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            if (await UnitOfWork.UserRepository.GetByEmailAsync(command.Email, cancellationToken) is not { } user)
            {
                return CommandResult.Unauthorized<CommandResult<JwtToken>>();
            }

            var signInResult = await _securityService.CheckPasswordAsync(user, command.Password);
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsEmailConfirmed)
                {
                    return CommandResult.Unauthorized<CommandResult<JwtToken>>(ReportCodeType.EmailNotConfirmed);
                }

                if (signInResult.IsPhoneNumberConfirmed)
                {
                    return CommandResult.Unauthorized<CommandResult<JwtToken>>(ReportCodeType.PhoneNumberNotConfirmed);
                }

                if (signInResult.IsLockedOut)
                {
                    user.Lock(_systemClock.UtcNow);

                    await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

                    return CommandResult.Unauthorized<CommandResult<JwtToken>>(ReportCodeType.UserIsLockedOut);
                }

                user.IncreaseAccessFailedCount();
                
                await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

                //await _mediator.Publish(new UserLoginFailedEvent(user.Id, request.Data.Login));

                return CommandResult.Unauthorized<CommandResult<JwtToken>>();
            }

            var jwtToken = _jwtProvider.Generate(user);

            user.ResetAccessFailedCount();
            user.IncreaseAccessCount(_systemClock.UtcNow);

            await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

            // await _mediator.Publish(new UserLoginEvent(user.Id));

            return CommandResult.Ok(jwtToken);
        }
    }

    /// <summary>
    /// Validator to validate request information about <see cref="User"/>.
    /// </summary>
    internal sealed class CommandValidator : CommandValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(command => command.Email)
                .NotNullOrEmpty();

            RuleFor(command => command.Password)
                .NotNullOrEmpty();
        }
    }
}