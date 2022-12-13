using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Application.Features.Commands.Todos
{
    public static class UpdateTodo
    {
        public sealed record Command(
            string Id,
            string Title,
            string Description,
            string ProjectId,
            DateTime? DueDate,
            [Required] int Priority,
            ICollection<string> Labels,
            OperationContext Context,
            bool BypassValidation = false) : Command<CommandResult<Todo>>(Context, BypassValidation);

        public sealed class CommandHandler : CommandHandler<Command, CommandResult<Todo>, IUnitOfWork>
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

            protected override async Task<CommandResult<Todo>> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var todo = await UnitOfWork.TodoRepository.GetByIdAsync(command.Id, cancellationToken);
                if (todo is null)
                {
                    return CommandResult.NotFound<CommandResult<Todo>>();
                }

                Mapper.Map(command, todo);

                todo.UpdatedBy = Membership.From(command.OperationContext.Identity);
                todo.UpdatedAt = _dateTimeProvider.UtcNow;

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
                RuleFor(request => request.Title)
                    .NotNullOrEmpty();
            }
        }
    }
}