using System.Collections.Generic;
using Acme.Todoist.Commons.Models;

namespace Acme.Todoist.Infrastructure.Queries
{
    public record PaginatedQueryResult<TData>(IEnumerable<TData> Data, PagingInfo PagingInfo, int StatusCode, ICollection<Report> Reports = null)
        : QueryResult<IEnumerable<TData>>(Data, StatusCode, Reports);
}
