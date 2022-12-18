using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Resources;
using Microsoft.AspNetCore.Http;

namespace Acme.Todoist.Application.Core.Commands
{
    public record CommandResult
    {
        public CommandResult()
        {
            Reports = new List<Report>();
        }

        public CommandResult(int statusCode, IEnumerable<Report> reports = null)
        {
            StatusCode = statusCode;
            Reports = reports ?? new Collection<Report>();
        }

        public CommandResult(int statusCode, Report report) : this(statusCode, new List<Report> { report }) { }

        /// <summary> Status of operation</summary>
        [JsonIgnore]
        public int StatusCode { get; internal init; }

        [JsonIgnore]
        public bool IsSuccessStatusCode => StatusCode is >= StatusCodes.Status200OK and <= 299;

        /// <summary>
        /// Detailed results of a method execution.
        /// </summary>
        public IEnumerable<Report> Reports { get; set; }

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 200 OK.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult Ok() => new(StatusCodes.Status200OK);

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 200 OK.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult<T> Ok<T>(T data) => new(data, StatusCodes.Status200OK);

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 202 Accepted.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult Accepted() => new(StatusCodes.Status202Accepted);

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 202 Accepted.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult<T> Accepted<T>(T data) => new(data, StatusCodes.Status202Accepted);

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 204 NoContent.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult NoContent() => new(StatusCodes.Status204NoContent);

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 204 NoContent.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult<T> NoContent<T>(T data) => new(data, StatusCodes.Status204NoContent);

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 200 OK.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult PartialContent<T>(T data, params Report[] reports) =>
            new CommandResult<T>(data, StatusCodes.Status206PartialContent, reports);

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 200 OK.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult PartialContent<T>(T data, IEnumerable<Report> reports) =>
            new CommandResult<T>(data, StatusCodes.Status206PartialContent, reports);

        /// <summary>
        ///  Create instance of <see cref="CommandResult{T}"/> with property status code 201 Created.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult{T}"/></returns>
        public static CommandResult<T> Created<T>(T data) => new(data, StatusCodes.Status201Created);

        /// <summary>
        ///  Create instance of <see cref="CommandResult{T}"/> with property status code 201 Created.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult{T}"/></returns>
        public static Task<CommandResult<T>> CreatedAsync<T>(T data) => Task.FromResult(new CommandResult<T>(data, StatusCodes.Status201Created));

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 404 Not found.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult NotFound() => new(StatusCodes.Status404NotFound);

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 404 Not found.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static TCommandResult NotFound<TCommandResult>()
            where TCommandResult : CommandResult, new()
        {
            return new TCommandResult { StatusCode = StatusCodes.Status404NotFound };
        }

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 422 Unprocessable entity.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult UnprocessableEntity(params Report[] reports)
        {
            return new CommandResult
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Reports = reports
            };
        }

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 422 Unprocessable entity.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult UnprocessableEntity(ReportCodeType reportCodeType) => UnprocessableEntity(Report.Create(reportCodeType));

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 422 Unprocessable entity.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static TCommandResult UnprocessableEntity<TCommandResult>(params Report[] reports)
            where TCommandResult : CommandResult, new()
        {
            return new TCommandResult
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Reports = reports
            };
        }

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 401 Not found.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static TCommandResult Unauthorized<TCommandResult>()
            where TCommandResult : CommandResult, new()
        {
            return new TCommandResult
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 422 Unprocessable entity.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static TCommandResult Unauthorized<TCommandResult>(params Report[] reports)
            where TCommandResult : CommandResult, new()
        {
            return new TCommandResult
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Reports = reports
            };
        }

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 403 Not found.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult Forbidden() => new(StatusCodes.Status403Forbidden);

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 422 Unprocessable entity.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static TCommandResult UnprocessableEntity<TCommandResult>(ReportCodeType reportCodeType)
            where TCommandResult : CommandResult, new() => UnprocessableEntity<TCommandResult>(Report.Create(reportCodeType));

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 500 internal server error.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static TCommandResult InternalServerError<TCommandResult>(Exception ex)
            where TCommandResult : CommandResult, new()
        {
            return new TCommandResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Reports = new Collection<Report>
                {
                    Report.Create(ReportCodeType.UnexpectedError)
                }
            };
        }

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 500 internal server error.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static CommandResult From(CommandResult command)
        {
            return new CommandResult
            {
                StatusCode = command.StatusCode,
                Reports = command.Reports
            };
        }

        /// <summary>
        /// Create instance of <see cref="CommandResult"/> with property status code 500 internal server error.
        /// </summary>
        /// <returns>Instance of <see cref="CommandResult"/></returns>
        public static TCommandResult From<TCommandResult>(CommandResult command)
            where TCommandResult : CommandResult, new()
        {
            return new TCommandResult
            {
                StatusCode = command.StatusCode,
                Reports = command.Reports
            };
        }
    }

    public record CommandResult<T> : CommandResult
    {
        public CommandResult() { }

        internal CommandResult(T data, int statusCode, IEnumerable<Report> reports = null) : base(statusCode, reports)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
