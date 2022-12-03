using Acme.Todoist.Infrastructure.Data;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Acme.Todoist.Data.TypeHandlers;

namespace Acme.Todoist.Data.Repositories
{
    /// <summary>
    /// Base class that should be inherited when implementing in concrete repositories.
    /// </summary>
    public abstract class Repository
    {
        private readonly IDbConnector _dbConnector;

        static Repository()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            //SqlMapper.OverrideHandlersForStandardTypes = true;

            // https://github.com/DapperLib/Dapper/issues/1715
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
            SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
        }

    protected Repository(IDbConnector dbConnector)
        {
            _dbConnector = dbConnector;

            // _dbConnector.Connection.TypeMapper.UseJsonNet()
        }

        /// <summary>
        /// Current connection.
        /// </summary>
        protected IDbConnection Connection => _dbConnector.Connection;

        /// <summary>
        /// Current transaction.
        /// </summary>
        protected IDbTransaction Transaction => _dbConnector.Transaction;

        /// <summary>
        /// Executes a query and returns the first cell selected.
        /// If the connection is holding an open transaction, command will be executed inside it.
        /// </summary>
        /// <typeparam name="TResult">Type of entity expected as the result.</typeparam>
        /// <param name="sql">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <returns>The first column of the first row of the result. If no records were selected, returns the default value for expected type.</returns>
        protected Task<TResult> ExecuteScalarWithTransactionAsync<TResult>(string sql, object parameters = null) => 
            Connection.ExecuteScalarAsync<TResult>(sql, parameters, Transaction);

        /// <summary>
        /// Executes a parameterized SQL command.
        /// If the connection is holding an open transaction, command will be executed inside it.
        /// </summary>
        /// <param name="sql">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        protected Task ExecuteWithTransactionAsync(string sql, object parameters = null) => 
            Connection.ExecuteAsync(sql, parameters, Transaction);

        /// <summary>
        /// Executes a query and returns the first record in the result typed as <typeparamref name="TResult"/>.
        /// If the connection is holding an open transaction, the command will be executed inside that transaction.
        /// </summary>
        /// <typeparam name="TResult">Type of entity expected as the result.</typeparam>
        /// <param name="sql">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <returns>The first record found. If not found, returns the default value for expected type.</returns>
        protected Task<TResult> FirstOrDefaultWithTransaction<TResult>(string sql, object parameters = null) => 
            Connection.QueryFirstOrDefaultAsync<TResult>(sql, parameters, Transaction);

        /// <summary>
        /// Executes a query and returns true if there's at least one record in the result.
        /// It has nothing to do with SQL EXISTS operator.
        /// If the connection is holding an open transaction, command will be executed inside it.
        /// </summary>
        /// <param name="sql">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <returns>True if found, false otherwise.</returns>
        protected Task<bool> ExistsWithTransactionAsync(string sql, object parameters = null) => 
            Connection.ExecuteScalarAsync<bool>(sql, parameters, Transaction);

        /// <summary>
        /// Executes a query and returns all records found.
        /// If the connection is holding an open transaction, command will be executed inside it.
        /// </summary>
        /// <typeparam name="TResult">Type of entity expected for each item in the result.</typeparam>
        /// <param name="sql">SQL command to be executed.</param>
        /// <param name="parameters">SQL parameters.</param>
        /// <returns>List of records selected.</returns>
        protected Task<IEnumerable<TResult>> ToListWithTransactionAsync<TResult>(string sql, object parameters = null) => 
            Connection.QueryAsync<TResult>(sql, parameters, Transaction);
    }
}
