using Acme.Todoist.Application.Core.Queries;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Features.Todos
{
    public sealed class GetTodoDetails
    {
        public sealed record Query(string Id, OperationContext OperationContext) : Query<QueryResult<Todo>>(OperationContext);

        public sealed class QueryHandler : IQueryHandler<Query, QueryResult<Todo>>
        {
            private readonly ITodoRepository _todoRepository;

            public QueryHandler(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository;
            }

            public async Task<QueryResult<Todo>> Handle(Query query, CancellationToken cancellationToken) =>
                QueryResult.OkOrNotFound(await _todoRepository.GetByIdAsync(query.Id, cancellationToken));
        }
    }
}
