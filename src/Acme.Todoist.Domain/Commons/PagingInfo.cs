using System;

namespace Acme.Todoist.Domain.Commons
{
    /// <summary>
    /// Represent a paging info.
    /// </summary>
    /// <param name="PageNumber">Page number.</param>
    /// <param name="RecordsPerPage">Number of records per page.</param>
    /// <param name="TotalRecords">Total records.</param>
    public sealed record PagingInfo(int PageNumber, int RecordsPerPage, long TotalRecords)
    {
        /// <summary>
        /// Total pages.
        /// </summary>
        public int TotalPages
        {
            get
            {
                var totalPages = (int)Math.Ceiling((double)TotalRecords / RecordsPerPage);
                return totalPages == 0 ? 1 : totalPages;
            }
        }

        public static PagingInfo Create(PagingParameters pagingParameters, PaginatedResult paginatedResult) =>
            new(pagingParameters.PageNumber, pagingParameters.RecordsPerPage, paginatedResult.TotalRecords);
    }
}
