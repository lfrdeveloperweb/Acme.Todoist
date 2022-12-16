using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.Core.Commons;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Security;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Application.Features.Todos
{
    public static class CreateTodo
    {
        public sealed record Command(
            string Title,
            string Description,
            string ProjectId,
            DateTime? DueDate,
            [Required] int Priority,
            ICollection<string> Labels,
            OperationContext Context,
            bool BypassValidation = false) : Command<CommandResult<Todo>>(Context, BypassValidation);

        public sealed class CommandHandler : CommandHandler<Command, CommandResult<Todo>>
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
                todo.CreatedAt = _dateTimeProvider.UtcNow;

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