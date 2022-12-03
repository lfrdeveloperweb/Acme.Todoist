using Acme.Todoist.Domain.Models;
using Acme.Todoist.Infrastructure.Data;
using System;

namespace Acme.Todoist.Core.Repositories
{
    /// <summary>
    /// Unit of work of repositories.
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkBase, IDisposable
    {
        /// <summary>
        /// Repository to handle information about <see cref="Todo"/>
        /// </summary>
        ITodoRepository TodoRepository { get; }
    }
}
