using Acme.Todoist.Commons.Models;
using Acme.Todoist.Core.Repositories;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Models.Filters;
using Acme.Todoist.Infrastructure.Data;
using Dapper;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Data.Repositories
{
    public sealed class TodoRepository : Repository, ITodoRepository
    {
        private const string BaseSelectCommandText = @"
                SELECT t.todo_id as Id
                     , t.title
                     , t.project_id
                     , t.priority
                     , t.due_date
                     , t.completed_at
                     , t.created_at
                     --, t.created_by
                  FROM todo t";

        public TodoRepository(IDbConnector dbConnector) : base(dbConnector) { }

        /// <inheritdoc />
        public Task<Todo> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            const string commandText = $"{BaseSelectCommandText} WHERE t.todo_id = @Id";

            return FirstOrDefaultWithTransaction<Todo>(commandText, new { Id = id }, cancellationToken);
        }
        
        public async Task<PaginatedResult<Todo>> ListPaginatedByFilterAsync(TodoFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken)
        {
            var commandText = new StringBuilder($@"
               SELECT COUNT(t.todo_id)
                 FROM todo t @DynamicFilter;

              {BaseSelectCommandText} @DynamicFilter
               ORDER BY t.created_at
                 OFFSET @Offset ROWS
             FETCH NEXT @RecordsPerPage ROWS ONLY;");

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { Offset = pagingParameters.Offset, RecordsPerPage = pagingParameters.RecordsPerPage });

            ApplyFilter(commandText, filter, parameters);

            using var multiQuery = await base.Connection.QueryMultipleAsync(
                new CommandDefinition(commandText.ToString(), parameters, Transaction, cancellationToken: cancellationToken));

            var totalRecords = await multiQuery.ReadSingleAsync<int>();
            var data = multiQuery.Read<Todo>();

            return new PaginatedResult<Todo>(data.ToList(), totalRecords);
        }

        public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
        {
            const string commandText =
                @"SELECT 1 FROM todo WHERE todo_id = @Id;";

            return ExistsWithTransactionAsync(commandText, new { Id = id }, cancellationToken);
        }

        public Task CreateAsync(Todo todo, CancellationToken cancellationToken)
        {
            const string query = @"
                INSERT INTO Course
                (
                    todo_id,
                    title,
                    project_id,
                    priority,
                    due_date,
                    created_at,
                    created_by
                ) 
                VALUES 
                (
                    @Id,
                    @Title,
                    @ProjectId,
                    @Priority,
                    @DueDate,
                    @CreatedAt,
                    @CreatedBy
                );";

            return ExecuteWithTransactionAsync(query, new
            {
                Id = todo.Id,
                Title = todo.Title,
                ProjectId = todo.Project?.Id,
                Priority = todo.Priority,
                DueDate = todo.DueDate,
                CreatedAt = todo.CreatedAt,
                CreatedBy = todo.CreatedBy?.MembershipId
            }, cancellationToken);
        }

        public Task DeleteAsync(Todo todo, CancellationToken cancellationToken)
        {
            const string commandText = "DELETE FROM todo WHERE todo_id = @Id;";

            return ExecuteWithTransactionAsync(commandText, new { Id = todo.Id }, cancellationToken);
        }

        private static void ApplyFilter(StringBuilder sql, TodoFilter filter, DynamicParameters parameters)
        {
            var conditions = new Collection<string>();

            if (filter.IsCompleted.HasValue)
            {
                conditions.Add($"t.completed_at is {(filter.IsCompleted.Value ? "not null" : "null")}");
            }

            // Put everything together in the WHERE clause
            var dynamicFilter = conditions.Any() ? $" WHERE {string.Join(" AND ", conditions)}" : "";

            sql.Replace("@DynamicFilter", dynamicFilter);
        }
    }
}
