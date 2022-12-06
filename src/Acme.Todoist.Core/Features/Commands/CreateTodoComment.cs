using Acme.Todoist.Commons.Models.Security;
using Acme.Todoist.Core.Repositories;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Infrastructure.Commands;
using Acme.Todoist.Infrastructure.Models;
using Acme.Todoist.Infrastructure.Utils;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Core.Features.Commands;

public static class CreateTodoComment
{
    public sealed record Command(
        string TodoId, 
        string Description, 
        OperationContext Context) : Command<CommandResult<TodoComment>>(Context);

    public sealed class CommandHandler : CommandHandler<Command, CommandResult<TodoComment>, IUnitOfWork>
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

        protected override async Task<CommandResult<TodoComment>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
        {
            var todoComment = Mapper.Map<TodoComment>(command);

            todoComment.Id = _keyGenerator.Generate();
            todoComment.CreatedBy = Membership.From(command.OperationContext.Identity);
            todoComment.CreatedAt = _dateTimeProvider.UtcNow;

            await UnitOfWork.TodoRepository.CreateCommentAsync(command.TodoId, todoComment, cancellationToken);

            return CommandResult.Created(todoComment);
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