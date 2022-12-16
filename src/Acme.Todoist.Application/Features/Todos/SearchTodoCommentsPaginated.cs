using Acme.Todoist.Application.Core.Queries;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Models.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Features.Todos;

public static class SearchTodoCommentsPaginated
{
    public record Query(string TodoId, PagingParameters PagingParameters, OperationContext OperationContext)
        : PaginatedQuery<PaginatedQueryResult<TodoComment>, TodoComment>(PagingParameters, OperationContext);

    public sealed class QueryHandler : IQueryHandler<Query, PaginatedQueryResult<TodoComment>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedQueryResult<TodoComment>> Handle(Query query, CancellationToken cancellationToken)
        {
            var filter = new TodoCommentFilter
            {
                TodoId = query.TodoId
            };

            var pagedResult = await _unitOfWork.TodoRepository.ListCommentsPaginatedByFilterAsync(filter, query.PagingParameters, cancellationToken);

            return QueryResult.Ok(pagedResult.Results, query.PagingParameters, pagedResult);
        }
    }
}