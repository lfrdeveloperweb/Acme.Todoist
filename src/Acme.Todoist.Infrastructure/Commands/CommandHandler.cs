using Acme.Todoist.Infrastructure.Data;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Infrastructure.Commands
{
    public abstract class CommandHandler<TCommand, TCommandResult, TUnitOfWork> : IRequestHandler<TCommand, TCommandResult>
        where TCommandResult : CommandResult, new()
        where TCommand : Command<TCommandResult>
        where TUnitOfWork : IUnitOfWorkBase
    {
        private readonly ICommandValidator<TCommand> _validator;

        protected CommandHandler(
            ILoggerFactory loggerFactory,
            TUnitOfWork unitOfWork,
            ICommandValidator<TCommand> validator = null,
            IMapper mapper = null)
        {
            _validator = validator;

            Logger = loggerFactory.CreateLogger(GetType());
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        protected ILogger Logger { get; }

        protected TUnitOfWork UnitOfWork { get; }

        protected IMapper Mapper { get; }

        // protected IEventDispatcher EventDispatcher { get; }

        public async Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var commandResult = new TCommandResult();

            try
            {
                await NormalizeCommand(command, cancellationToken);

                if (_validator is not null)
                {
                    var commandValidationResult = await _validator.ValidateCommandAsync(command);
                    if (!commandValidationResult.IsValid)
                    {
                        return CommandResult.UnprocessableEntity<TCommandResult>(commandValidationResult.Reports.ToArray());
                    }
                }

                commandResult = await ProcessCommandAsync(command, cancellationToken);

                if (commandResult.IsSuccessStatusCode)
                {

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
    public abstract class CommandHandler<TCommand, TUnitOfWork> : CommandHandler<TCommand, CommandResult, TUnitOfWork>
        where TCommand : Command<CommandResult>
        where TUnitOfWork : IUnitOfWorkBase
    {
        // constructors
        protected CommandHandler(
            ILoggerFactory loggerFactory,
            TUnitOfWork unitOfWork,
            CommandValidator<TCommand> validator = null,
            IMapper mapper = null) : base(loggerFactory, unitOfWork, validator, mapper) { }
    }
}
