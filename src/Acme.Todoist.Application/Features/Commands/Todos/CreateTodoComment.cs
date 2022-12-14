using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Security;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Features.Commands.Todos;

public static class CreateTodoComment
{
    public sealed record Command(
        string TodoId,
        string Description,
        OperationContext Context) : Command<CommandResult<TodoComment>>(Context);

    public sealed class CommandHandler : CommandHandler<Command, CommandResult<TodoComment>>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<Command> validator,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider) : base(loggerFactory, unitOfWork, validator, mapper: mapper)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        protected override async Task<CommandResult<TodoComment>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var comment = Mapper.Map<TodoComment>(command);

            comment.TodoId = command.TodoId;
            comment.CreatedBy = Membership.From(command.OperationContext.Identity);
            comment.CreatedAt = _dateTimeProvider.UtcNow;

            await UnitOfWork.TodoRepository.CreateCommentAsync(comment, cancellationToken);

            return CommandResult.Created(comment);
        }
    }

    /// <summary>
    /// Validator to validate request information about <see cref="TodoComment"/>.
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
            //Transform(it => it.Title, it => it.Trim())
            //    .NotNullOrEmpty();

            //Transform(it => it.Description, it => it.Trim())
            //    .NotNullOrEmpty();

            //RuleFor(command => command.Level)
            //    .NotNullOrEmpty()
            //    .Must(level => Enum.IsDefined(typeof(CourseLevel), level));
            //.WithMessageFromErrorCode(ReportCodeType.InvalidCourseLevel);

            //RuleFor(request => request.Name)
            //    .NotNullOrEmpty();

            RuleFor(request => request)
                .CustomAsync(CanCreate);
        }

        /// <summary>
        /// Validate if can create TodoComment.
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