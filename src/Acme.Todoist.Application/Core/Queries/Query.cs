using Acme.Todoist.Domain.Commons;
using MediatR;

namespace Acme.Todoist.Application.Core.Queries
{
    public interface IQuery<out TQueryResult> : IRequest<TQueryResult>
        where TQueryResult : IQueryResult { }

    public record Query<TQueryResult>(OperationContext OperationContext, bool BypassValidation = false) : IQuery<TQueryResult>
        where TQueryResult : IQueryResult;

    public record PaginatedQuery<TQueryResult, TData>(PagingParameters PagingParameters, OperationContext OperationContext) : Query<TQueryResult>(OperationContext)
        where TQueryResult : PaginatedQueryResult<TData>;
}