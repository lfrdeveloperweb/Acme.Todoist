﻿using Acme.Todoist.Data.TypeHandlers;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Data.Repositories
{
    /// <summary>
    /// Base class that should be inherited when implementing in concrete repositories.
    /// </summary>
    public abstract class Repository
    {
        static Repository()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            //SqlMapper.OverrideHandlersForStandardTypes = true;

            // https://github.com/DapperLib/Dapper/issues/1715
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
            SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
        }

        protected Repository(DbContext context)
        {
            Context = context;

            // _dbConnector.Connection.TypeMapper.UseJsonNet()
        }

        protected DbContext Context { get; }

        /// <summary>
        /// Current connection.
        /// </summary>
        protected IDbConnection Connection => Context.Database.GetDbConnection();

        /// <summary>
        /// Current transaction.
        /// </summary>
        protected IDbTransaction Transaction => (Context.Database.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance;

        /// <summary>
        /// Executes a query and returns the first cell selected.
        /// If the connection is holding an open transaction, command will be executed inside it.
        /// </summary>
        /// <typeparam name="TResult">Type of entity expected as the result.</typeparam>
        /// <param name="commandText">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <param name="cancellationToken">The cancellation token for this operation.</param>
        /// <returns>The first column of the first row of the result. If no records were selected, returns the default value for expected type.</returns>
        protected Task<TResult> ExecuteScalarWithTransactionAsync<TResult>(string commandText, object parameters = null, CancellationToken cancellationToken = default) =>
            Connection.ExecuteScalarAsync<TResult>(new CommandDefinition(commandText, parameters, Transaction, cancellationToken: cancellationToken));

        /// <summary>
        /// Executes a parameterized SQL command.
        /// If the connection is holding an open transaction, command will be executed inside it.
        /// </summary>
        /// <param name="commandText">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <param name="cancellationToken">The cancellation token for this operation.</param>
        protected Task ExecuteWithTransactionAsync(string commandText, object parameters = null, CancellationToken cancellationToken = default) =>
            Connection.ExecuteAsync(new CommandDefinition(commandText, parameters, Transaction, cancellationToken: cancellationToken));

        /// <summary>
        /// Executes a query and returns the first record in the result typed as <typeparamref name="TResult"/>.
        /// If the connection is holding an open transaction, the command will be executed inside that transaction.
        /// </summary>
        /// <typeparam name="TResult">Type of entity expected as the result.</typeparam>
        /// <param name="commandText">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <param name="cancellationToken">The cancellation token for this operation.</param>
        /// <returns>The first record found. If not found, returns the default value for expected type.</returns>
        protected Task<TResult> FirstOrDefaultWithTransaction<TResult>(string commandText, object parameters = null, CancellationToken cancellationToken = default) =>
            Connection.QueryFirstOrDefaultAsync<TResult>(new CommandDefinition(commandText, parameters, Transaction, cancellationToken: cancellationToken));

        /// <summary>
        /// Executes a query and returns true if there's at least one record in the result.
        /// It has nothing to do with SQL EXISTS operator.
        /// If the connection is holding an open transaction, command will be executed inside it.
        /// </summary>
        /// <param name="commandText">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <param name="cancellationToken">The cancellation token for this operation.</param>
        /// <returns>True if found, false otherwise.</returns>
        protected Task<bool> ExistsWithTransactionAsync(string commandText, object parameters = null, CancellationToken cancellationToken = default) =>
            Connection.ExecuteScalarAsync<bool>(new CommandDefinition(commandText, parameters, Transaction, cancellationToken: cancellationToken));

        /// <summary>
        /// Executes a query and returns all records found.
        /// If the connection is holding an open transaction, command will be executed inside it.
        /// </summary>
        /// <typeparam name="TResult">Type of entity expected for each item in the result.</typeparam>
        /// <param name="commandText">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <param name="cancellationToken">The cancellation token for this operation.</param>
        /// <returns>List of records selected.</returns>
        protected Task<IEnumerable<TResult>> ToListWithTransactionAsync<TResult>(string commandText, object parameters = null, CancellationToken cancellationToken = default) =>
            Connection.QueryAsync<TResult>(new CommandDefinition(commandText, parameters, Transaction, cancellationToken: cancellationToken));
    }

    public abstract class Repository<TEntity> : Repository
        where TEntity : class
    {
        protected Repository(DbContext context)
            : base(context)
        {
            DbSet = Context.Set<TEntity>();
            DbSetAsNoTracking = Context.Set<TEntity>()
                .AsNoTrackingWithIdentityResolution();
        }

        protected virtual IQueryable<TEntity> DbSetAsNoTracking { get; }

        protected DbSet<TEntity> DbSet { get; }

        public virtual async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            await DbSet.FindAsync(new object[] { id }, cancellationToken);
    }
}
