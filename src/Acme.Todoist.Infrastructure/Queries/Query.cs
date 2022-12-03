using Acme.Todoist.Commons.Models;
using Acme.Todoist.Infrastructure.Models;
using MediatR;

namespace Acme.Todoist.Infrastructure.Queries
{
    public interface IQuery<out TQueryResult> : IRequest<TQueryResult>
        where TQueryResult : IQueryResult { }

    public record Query<TQueryResult>(OperationContext OperationContext, bool BypassValidation = false) : IQuery<TQueryResult>
        where TQueryResult : IQueryResult;

    public record PaginatedQuery<TQueryResult, TData>(PagingParameters PagingParameters, OperationContext OperationContext) : Query<TQueryResult>(OperationContext)
        where TQueryResult : PaginatedQueryResult<TData>;
}