using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Resources;
using Microsoft.AspNetCore.Http;

namespace Acme.Todoist.Application.Core.Queries
{
    public interface IQueryResult
    {
        int StatusCode { get; init; }

        ICollection<Report> Reports { get; init; }
    }

    public record QueryResult(int StatusCode, ICollection<Report> Reports) : IQueryResult
    {
        /// <summary>
        /// Create instance of <see cref="QueryResult{TData}"/> with property status code 200 OK.
        /// </summary>
        /// <returns>Instance of <see cref="QueryResult{TData}"/></returns>
        public static QueryResult<TData> Ok<TData>(TData data) => new(data, StatusCodes.Status200OK);

        /// <summary>
        /// Create instance of <see cref="QueryResult{TData}"/> with property status code 404 NotFound.
        /// </summary>
        /// <returns>Instance of <see cref="QueryResult{TData}"/></returns>
        public static QueryResult<TData> NotFound<TData>() => new(default, StatusCodes.Status404NotFound);

        /// <summary>
        /// Create instance of <see cref="QueryResult{TData}"/> with property status code 202 OK if data is not null.
        /// Otherwise, create instance of <see cref="QueryResult{TData}"/> with property status code 404 NotFound.
        /// </summary>
        /// <returns>Instance of <see cref="QueryResult{TData}"/></returns>
        public static QueryResult<TData> OkOrNotFound<TData>(TData data) => data != null ? Ok(data) : NotFound<TData>();

        /// <summary>
        /// Create instance of <see cref="IQueryResult"/> with property status code 500 internal server error.
        /// </summary>
        /// <returns>Instance of <see cref="IQueryResult"/></returns>
        public static TQueryResult InternalServerError<TQueryResult>(Exception ex)
            where TQueryResult : IQueryResult, new()
        {
            return new TQueryResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Reports = new Collection<Report>
                {
                    Report.Create(ReportCodeType.UnexpectedError)
                }
            };
        }

        /// <summary>
        /// Create instance of ListPagedOperationResponse with property State 200 OK.
        /// </summary>
        public static ListQueryResult<TData> Ok<TData>(IEnumerable<TData> data) => new(data, StatusCodes.Status200OK);

        /// <summary>
        /// Create instance of ListPagedOperationResponse with property State 200 OK.
        /// </summary>
        public static PaginatedQueryResult<TData> Ok<TData>(IEnumerable<TData> data, PagingParameters pagingParameters, PaginatedResult paginatedResult) =>
            new(data, PagingInfo.Create(pagingParameters, paginatedResult), StatusCodes.Status200OK);
    }

    public record QueryResult<TData>(TData Data, int StatusCode, ICollection<Report> Reports = null) : QueryResult(StatusCode, Reports);
}
