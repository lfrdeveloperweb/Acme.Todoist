using MediatR;

namespace Acme.Todoist.Application.Core.Queries
{
    public interface IQueryHandler<in TQuery, TQueryResult> : IRequestHandler<TQuery, TQueryResult>
        where TQuery : IQuery<TQueryResult>
        where TQueryResult : IQueryResult { }
}
