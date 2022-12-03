using System.Data;

namespace Acme.Todoist.Infrastructure.Data
{
    public interface IUnitOfWorkBase
    {
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
