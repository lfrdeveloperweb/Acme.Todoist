using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Repositories;

namespace Acme.Todoist.Application.Core.Commands
{
    public abstract class CommandHandler<TCommand, TCommandResult> : IRequestHandler<TCommand, TCommandResult>
        where TCommandResult : CommandResult, new()
        where TCommand : Command<TCommandResult>
    {
        private readonly ICommandValidator<TCommand> _validator;

        protected CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<TCommand> validator = null,
            IMapper mapper = null)
        {
            _validator = validator;

            Logger = loggerFactory.CreateLogger(GetType());
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        protected ILogger Logger { get; }

        protected IUnitOfWork UnitOfWork { get; }

        protected IMapper Mapper { get; }

        // protected IEventDispatcher EventDispatcher { get; }

        public async Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var commandResult = new TCommandResult();

            try
            {
                await NormalizeCommand(command, cancellationToken);

                if (!command.BypassValidation && _validator is not null)
                {
                    var commandValidationResult = await _validator.ValidateCommandAsync(command);
                    if (!commandValidationResult.IsValid)
                    {
                        return CommandResult.UnprocessableEntity<TCommandResult>(commandValidationResult.Reports.ToArray());
                    }
                }

                UnitOfWork.BeginTransaction();

                commandResult = await ProcessCommandAsync(command, cancellationToken);
                if (commandResult.IsSuccessStatusCode)
                {
                    UnitOfWork?.CommitTransactionAsync();
                }
                else
                {
                    UnitOfWork.RollbackTransaction();
                }
            }
            //catch (AbortCommandException ex)
            //{
            //    // await UnitOfWork.RollbackTransactionAsync();

            //    Logger.LogError("Abort operation exception: {ex}", ex.Message);

            //    return CommandResult.InternalServerError<TCommandResult>(ex);
            //}
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);

                UnitOfWork?.RollbackTransaction();

                commandResult = CommandResult.InternalServerError<TCommandResult>(ex);
            }
            finally
            {
                await ProcessCompletedAsync(command, commandResult, cancellationToken);
            }

            return commandResult;
        }

        /// <summary>
        /// Processes the operation. The implementation must define the processing logic.
        /// </summary>
        protected abstract Task<TCommandResult> ProcessCommandAsync(TCommand command, CancellationToken cancellationToken);

        /// <summary>
        /// Normalizes the <see cref="TCommand"/> before validation.
        /// </summary>
        protected virtual Task NormalizeCommand(TCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

        /// <summary>
        /// Normalizes the <see cref="TCommand"/> before validation.
        /// </summary>
        protected virtual Task ProcessCompletedAsync(TCommand command, TCommandResult commandResult, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    /// <summary>
    /// Template method design pattern is used to allow operation-specific validation and processing requests.
    /// </summary>
    public abstract class CommandHandler<TCommand> : CommandHandler<TCommand, CommandResult>
        where TCommand : Command<CommandResult>
    {
        // constructors
        protected CommandHandler(
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            ICommandValidator<TCommand> validator = null,
            IMapper mapper = null) : base(loggerFactory, unitOfWork, validator, mapper) { }
    }
}
