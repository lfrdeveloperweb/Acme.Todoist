using Acme.Todoist.Core.Repositories;
using Acme.Todoist.Infrastructure.Commands;
using Acme.Todoist.Infrastructure.Models;
using Acme.Todoist.Infrastructure.Utils;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Commons.Models.Security;
using Acme.Todoist.Infrastructure.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Acme.Todoist.Core.Features.Commands
{
    public static class CreateTodo
    {
        public sealed record Command(
            string Title,
            string ProjectId,
            DateTime? DueDate,
            [Required] int Priority,
            ICollection<string> Tags,
            OperationContext Context) : Command<CommandResult<Todo>>(Context);

        public sealed class CommandHandler : CommandHandler<Command, CommandResult<Todo>, IUnitOfWork>
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

            protected override async Task<CommandResult<Todo>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var todo = Mapper.Map<Todo>(command);

                todo.Id = _keyGenerator.Generate();
                todo.CreatedBy = Membership.From(command.OperationContext.Identity);
                todo.CreatedAt = _dateTimeProvider.BrasiliaNow;

                await UnitOfWork.TodoRepository.CreateAsync(todo, cancellationToken);

                return CommandResult.Created(todo);
            }
        }

        /// <summary>
        /// Validator to validate request information about <see cref="Todo"/>.
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
            /// Validate if can create Todo.
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
}
