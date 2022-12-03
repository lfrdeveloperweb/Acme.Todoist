using Acme.Todoist.Infrastructure.Data;
using System.Data;
using System.Data.SqlClient;

namespace Acme.Todoist.Data.Factories
{
    public class AcmeDatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public AcmeDatabaseConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        // public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
