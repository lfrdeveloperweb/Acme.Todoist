using System.Collections.Generic;
using Acme.Todoist.Domain.Commons;

namespace Acme.Todoist.Application.Core.Queries
{
    public record PaginatedQueryResult<TData>(IEnumerable<TData> Data, PagingInfo PagingInfo, int StatusCode, ICollection<Report> Reports = null)
        : QueryResult<IEnumerable<TData>>(Data, StatusCode, Reports);
}
