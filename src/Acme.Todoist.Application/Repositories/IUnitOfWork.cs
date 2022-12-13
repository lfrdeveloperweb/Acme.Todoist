﻿using System;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Infrastructure.Data;

namespace Acme.Todoist.Application.Repositories
{
    /// <summary>
    /// Unit of work of repositories.
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkBase, IDisposable
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
    }
}