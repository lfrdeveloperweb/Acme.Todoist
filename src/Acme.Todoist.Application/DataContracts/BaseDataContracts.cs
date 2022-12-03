using Acme.Todoist.Commons.Models;
using Acme.Todoist.Infrastructure.Commands;
using Acme.Todoist.Infrastructure.Queries;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Acme.Todoist.Application.DataContracts
{
    namespace Requests
    {

    }

    namespace Responses
    {
        /// <summary>
        /// The base class to be used as response from a method execution.
        /// </summary>
        public record Response
        {
            public Response() { }

            public Response(int statusCode, params Report[] reports)
            {
                StatusCode = statusCode;
                Reports = reports;
            }

            public int StatusCode { get; init; }

            public Report[] Reports { get; init; }

            public bool IsSuccessStatusCode => StatusCode is >= 200 and <= 299;
            
            /// <summary>
            /// Create instance of <see cref="Response"/> with property State "<see cref="StatusCodes.Status200OK"/>".
            /// </summary>
            public static Response Ok() => new(StatusCodes.Status200OK);

            /// <summary>
            /// Create instance of <see cref="Response"/>.
            /// </summary>
            public static Response<T> Ok<T>(T data) => new(data, StatusCodes.Status200OK);

            public static Response<TResponseData> From<TModel, TResponseData>(QueryResult<TModel> result, IMapper mapper) =>
                new(mapper.Map<TResponseData>(result.Data), result.StatusCode, result.Reports?.ToArray());

            public static ListResponse<TResponseData> From<TModel, TResponseData>(ListQueryResult<TModel> result, IMapper mapper) =>
                new(mapper.Map<IEnumerable<TResponseData>>(result.Data), result.StatusCode, result.Reports?.ToArray());

            public static PaginatedResponse<TResponseData> From<TModel, TResponseData>(PaginatedQueryResult<TModel> result, IMapper mapper) =>
                new(mapper.Map<IEnumerable<TResponseData>>(result.Data), result.PagingInfo, result.StatusCode, result.Reports?.ToArray());

            public static Response From(CommandResult result) => new(result.StatusCode, result.Reports?.ToArray());

            public static Response<TResponseData> From<TModel, TResponseData>(CommandResult<TModel> result, IMapper mapper) =>
                new(mapper.Map<TResponseData>(result.Data), result.StatusCode, result.Reports?.ToArray());
        }

        /// <summary>
        /// Represents the return of an internal operation.
        /// </summary>
        /// <typeparam name="T">Type of data to be returned.</typeparam>
        public record Response<T> : Response
        {
            public Response() { }

            public Response(T data, int statusCode, params Report[] reports) : base(statusCode, reports)
            {
                Data = data;
            }

            public T Data { get; init; }
        }

        public record ListResponse<TData> : Response<IEnumerable<TData>>
        {
            public ListResponse() { }

            public ListResponse(IEnumerable<TData> data, int statusCode, params Report[] reports)
                : base(data, statusCode, reports) { }
        }

        public record PaginatedResponse<TData> : Response<IEnumerable<TData>>
        {
            public PaginatedResponse() { }

            public PaginatedResponse(IEnumerable<TData> data, PagingInfo pagingInfo, int statusCode, params Report[] reports)
                : base(data, statusCode, reports)
            {
                PagingInfo = pagingInfo;
            }

            public PagingInfo PagingInfo { get; init; }
        }
    }
}
