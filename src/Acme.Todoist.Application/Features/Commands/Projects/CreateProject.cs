using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Commons.Models.Security;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Infrastructure.Commands;
using Acme.Todoist.Infrastructure.Extensions;
using Acme.Todoist.Infrastructure.Models;
using Acme.Todoist.Infrastructure.Utils;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Application.Features.Commands.Projects;

public static class CreateProject
{
    public sealed record Command(
        string Title,
        string Color,
        OperationContext Context) : Command<CommandResult<Project>>(Context);

    public sealed class CommandHandler : CommandHandler<Command, CommandResult<Project>, IUnitOfWork>
    {
        private readonly IKeyGenerator _keyGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<Command> validator,
            IMapper mapper,
            IKeyGenerator keyGenerator,
            IDateTimeProvider dateTimeProvider) : base(loggerFactory, unitOfWork, validator, mapper: mapper)
        {
            _keyGenerator = keyGenerator;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override async Task<CommandResult<Project>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var project = Mapper.Map<Project>(command);

            project.Id = _keyGenerator.Generate();
            project.CreatedBy = Membership.From(command.OperationContext.Identity);
            project.CreatedAt = _dateTimeProvider.BrasiliaNow;

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
        private readonly IDateTimeProvider _dateTimeProvider;

        public CommandValidator(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;

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