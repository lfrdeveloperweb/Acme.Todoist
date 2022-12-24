using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Application.Extensions;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Security;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Application.Features.Projects;

public static class CreateProject
{
    public sealed record Command(
        string Title,
        string Color,
        OperationContext Context) : Command<CommandResult<Project>>(Context);

    public sealed class CommandHandler : CommandHandler<Command, CommandResult<Project>>
    {
        private readonly IKeyGenerator _keyGenerator;
        private readonly ISystemClock _systemClock;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<Command> validator,
            IMapper mapper,
            IKeyGenerator keyGenerator,
            ISystemClock systemClock) : base(loggerFactory, unitOfWork, validator, mapper: mapper)
        {
            _keyGenerator = keyGenerator;
            _systemClock = systemClock;
        }

        protected override async Task<CommandResult<Project>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var project = Mapper.Map<Project>(command);

            project.Id = _keyGenerator.Generate();
            project.CreatedBy = Membership.From(command.OperationContext.Identity);
            project.CreatedAt = _systemClock.UtcNow;

            await UnitOfWork.ProjectRepository.CreateAsync(project, cancellationToken);

            return CommandResult.Created(project);
        }
    }

    /// <summary>
    /// Validator to validate request information about <see cref="Project"/>.
    /// </summary>
    public sealed class CommandValidator : CommandValidator<Command>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemClock _systemClock;

        public CommandValidator(IUnitOfWork unitOfWork, ISystemClock systemClock)
        {
            _unitOfWork = unitOfWork;
            _systemClock = systemClock;

            SetupValidation();
        }

        private void SetupValidation()
        {
            Transform(it => it.Title, it => it.Trim())
                .NotNullOrEmpty();

            RuleFor(request => request)
                .CustomAsync(CanCreate);
        }

        /// <summary>
        /// Validate if can create Project.
        /// </summary>
        private Task CanCreate(Command command, ValidationContext<Command> validationContext, CancellationToken cancellationToken)
        {
            //if (!RequestContext.Membership.Roles.Contains(Common.Models.Security.Role.Manager) && !RequestContext.Membership.IsSuperAdmin)
            //{
            //    validationContext.AddFailure("User", ReportCodeType.OnlyManagerIsAllowedToDoThisOperation);
            //    return;
            //}

            return Task.CompletedTask;
        }
    }
}