using Acme.Todoist.Domain.Models;
using System;
using System.Data;

namespace Acme.Todoist.Application.Repositories
{
    /// <summary>
    /// Unit of work of repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repository to handle information about <see cref="User"/>
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// Repository to handle information about <see cref="Project"/>
        /// </summary>
        IProjectRepository ProjectRepository { get; }

        /// <summary>
        /// Repository to handle information about <see cref="Todo"/>
        /// </summary>
        ITodoRepository TodoRepository { get; }

        /// <summary>
        /// Initiates a transaction under the connection held an instance of UnitOfWork.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level. Default: <see cref="IsolationLevel.ReadCommitted"/>.</param>
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        void RollbackTransaction();
    }
}
