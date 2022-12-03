using Acme.Todoist.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Acme.Todoist.Data.Connectors
{
    /// <summary>
    /// Responsible for handling a PostgreSql connection, processing database transactions and ensuring the consistency of the transaction.
    /// </summary>
    public class PostgreSqlConnector : IDbConnector
    {
        private readonly ILogger<PostgreSqlConnector> _logger;
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;
        private bool _disposed;

        public PostgreSqlConnector(ILogger<PostgreSqlConnector> logger, IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _logger = logger;
            _databaseConnectionFactory = databaseConnectionFactory;

            Connection = _databaseConnectionFactory.CreateConnection();
        }

        /// <summary>
        /// Database connection, through which all transactions will be committed to.
        /// </summary>
        public IDbConnection Connection { get; }

        /// <summary>
        /// Database transaction associated with the connection.
        /// </summary>
        public IDbTransaction Transaction { get; private set; }

        /// <summary>
        /// Begins a transaction in the <see cref="Connection"/> source.
        /// It's useful to guarantee the consistency of all operations made within
        /// the transaction, enabling rolling back all scripts ran in case of error.
        /// </summary>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (Transaction != null)
            {
                return Transaction;
            }

            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

            return (Transaction = Connection.BeginTransaction(isolationLevel));
        }

        /// <summary>
        /// Responsible for check if a connection string to work successful.
        /// </summary>
        async Task<HealthCheckResult> IHealthCheck.CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = _databaseConnectionFactory.CreateConnection())
                {
                    const string sql = @"
                        SELECT current_user
	                         , version() as version 
	                         , current_database() as database
	                         , inet_server_addr() as host
	                         , inet_server_port() as port
	                         , current_schemas(false) as schemas";

                    var databaseInfo = await connection.QueryFirstAsync<PostgreSqlInfo>(sql);

                    return HealthCheckResult.Healthy(
                        description: "PostgresSql database",
                        data: new Dictionary<string, object>
                        {
                            [nameof(PostgreSqlInfo.Version)] = databaseInfo.Version,
                            [nameof(PostgreSqlInfo.Host)] = $"{databaseInfo.Host}:{databaseInfo.Port}",
                            [nameof(PostgreSqlInfo.Database)] = databaseInfo.Database,
                            [nameof(PostgreSqlInfo.CurrentUser)] = databaseInfo.CurrentUser,
                            [nameof(PostgreSqlInfo.Schemas)] = databaseInfo.Schemas
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }

        /// <summary>
        /// Dispose the resources used along the lifecycle of this instance.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            Transaction?.Dispose();
            Connection?.Dispose();

            GC.SuppressFinalize(this);

            _disposed = true;
        }

        private sealed record PostgreSqlInfo
        {
            public string CurrentUser { get; set; }
            public string Version { get; set; }
            public string Database { get; set; }
            public IPAddress Host { get; set; }
            public string Port { get; set; }
            public string[] Schemas { get; set; }
        }
    }
}
