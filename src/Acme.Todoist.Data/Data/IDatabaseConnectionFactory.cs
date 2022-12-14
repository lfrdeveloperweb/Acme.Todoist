using System.Data;

namespace Acme.Todoist.Infrastructure.Data
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}