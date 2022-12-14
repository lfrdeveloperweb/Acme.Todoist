using System.Collections.Generic;
using Acme.Todoist.Domain.Commons;

namespace Acme.Todoist.Application.Core.Queries
{
    public record ListQueryResult<TData>(IEnumerable<TData> Data, int StatusCode, ICollection<Report> Reports = null)
        : QueryResult<IEnumerable<TData>>(Data, StatusCode, Reports);
}
