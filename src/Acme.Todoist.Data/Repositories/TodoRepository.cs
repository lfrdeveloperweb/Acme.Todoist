using Acme.Todoist.Commons.Models;
using Acme.Todoist.Core.Repositories;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Models.Filters;
using Acme.Todoist.Infrastructure.Data;
using Dapper;
using Newtonsoft.Json;
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
                 , t.description
                 , t.project_id
                 , t.priority
                 , t.due_date
                 , t.completed_at
                 , t.created_at
                 , t.updated_at
                 , t.deleted_at  
                 , t.created_by
                 , t.updated_by                 
              FROM todo t";

        private const string CommentBaseSelectCommandText = @"
            SELECT c.todo_comment_id as Id
                 , c.todo_id
                 , c.description
                 , c.created_at
                 --, c.created_by                     
              FROM todo_comment c";

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
            const string commandText = @"
                INSERT INTO todo
                (
                    todo_id,
                    title,
                    description,
                    project_id,
                    priority,
                    due_date,
                    labels,
                    created_at,
                    created_by
                ) 
                VALUES 
                (
                    @Id,
                    @Title,
                    @Description,
                    @ProjectId,
                    @Priority,
                    @DueDate,
                    @Labels::json,
                    @CreatedAt,
                    @CreatedBy
                );";

            return ExecuteWithTransactionAsync(commandText, new
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                ProjectId = todo.Project?.Id,
                Priority = todo.Priority,
                DueDate = todo.DueDate,
                Labels = JsonConvert.SerializeObject(todo.Labels, Formatting.None),
                CreatedAt = todo.CreatedAt,
                CreatedBy = todo.CreatedBy?.MembershipId
            }, cancellationToken);
        }

        public Task UpdateAsync(Todo todo, CancellationToken cancellationToken)
        {
            const string commandText = @"
                UPDATE todo
                   SET title = @Title
                     , description = @Description
                     , project_id = @ProjectId
                     , priority = @Priority
                     , due_date = @DueDate
                     , labels = @Labels
                     , updated_by = @UpdatedBy
                     , updated_at = @UpdatedAt
                 WHERE todo_id = @Id;";

            return ExecuteWithTransactionAsync(commandText, new
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                ProjectId = todo.Project?.Id,
                Priority = todo.Priority,
                DueDate = todo.DueDate,
                Labels = JsonConvert.SerializeObject(todo.Labels, Formatting.None),
                UpdatedAt = todo.UpdatedAt,
                UpdatedBy = todo.UpdatedBy?.MembershipId
            }, cancellationToken);
        }

        public Task DeleteAsync(Todo todo, CancellationToken cancellationToken)
        {
            const string commandText = @"
                UPDATE todo
                   SET deleted_at = CURRENT_TIMESTAMP
                 WHERE todo_id = @Id;";

            return ExecuteWithTransactionAsync(commandText, cancellationToken, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TodoComment> GetCommentByIdAsync(string id, CancellationToken cancellationToken)
        {
            const string commandText = $"{CommentBaseSelectCommandText} WHERE c.todo_comment_id = @Id";

            return FirstOrDefaultWithTransaction<TodoComment>(commandText, new { Id = id }, cancellationToken);
        }

        public async Task<PaginatedResult<TodoComment>> ListCommentsPaginatedByFilterAsync(TodoCommentFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken)
        {
            var commandText = new StringBuilder($@"
               SELECT COUNT(c.todo_id)
                 FROM todo_comment c @DynamicFilter;

              {CommentBaseSelectCommandText} @DynamicFilter
               ORDER BY c.created_at
                 OFFSET @Offset ROWS
             FETCH NEXT @RecordsPerPage ROWS ONLY;");

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { Offset = pagingParameters.Offset, RecordsPerPage = pagingParameters.RecordsPerPage });

            CommentApplyFilter(commandText, filter, parameters);

            using var multiQuery = await base.Connection.QueryMultipleAsync(
                new CommandDefinition(commandText.ToString(), parameters, Transaction, cancellationToken: cancellationToken));

            var totalRecords = await multiQuery.ReadSingleAsync<int>();
            var data = multiQuery.Read<TodoComment>();

            return new PaginatedResult<TodoComment>(data.ToList(), totalRecords);
        }

        public Task CreateCommentAsync(string todoId, TodoComment comment, CancellationToken cancellationToken)
        {
            const string commandText = @"
                INSERT INTO todo_comment
                (
                    todo_comment_id,
                    todo_id,
                    description,
                    created_at,
                    created_by
                ) 
                VALUES 
                (
                    @Id,
                    @TodoId,
                    @Description,
                    @CreatedAt,
                    @CreatedBy
                );";

            return ExecuteWithTransactionAsync(commandText, new
            {
                Id = comment.Id,
                TodoId = todoId,
                Description = comment.Description,
                CreatedAt = comment.CreatedAt,
                CreatedBy = comment.CreatedBy?.MembershipId
            }, cancellationToken);
        }

        public Task DeleteCommentAsync(TodoComment comment, CancellationToken cancellationToken)
        {
            const string commandText = "DELETE FROM todo_comment WHERE todo_comment_id = @Id;";

            return ExecuteWithTransactionAsync(commandText, new { Id = comment.Id }, cancellationToken);
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

        private static void CommentApplyFilter(StringBuilder sql, TodoCommentFilter filter, DynamicParameters parameters)
        {
            var conditions = new Collection<string>();

            conditions.Add("c.todo_id = @TodoId");
            parameters.Add("TodoId", filter.TodoId);

            // Put everything together in the WHERE clause
            var dynamicFilter = conditions.Any() ? $" WHERE {string.Join(" AND ", conditions)}" : "";

            sql.Replace("@DynamicFilter", dynamicFilter);
        }
    }
}
