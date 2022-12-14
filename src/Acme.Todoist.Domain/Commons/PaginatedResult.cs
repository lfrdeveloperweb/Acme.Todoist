using System.Collections.Generic;

namespace Acme.Todoist.Domain.Commons
{
    public abstract record PaginatedResult(long TotalRecords);

    /// <summary>
    /// Represents a paged result of an operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record PaginatedResult<T>(IEnumerable<T> Results, long TotalRecords) : PaginatedResult(TotalRecords);
}
