using Acme.Todoist.Domain.Resources;
using System.Collections.Generic;

namespace Acme.Todoist.Domain.Commons
{
    public record Result
    {
        protected Result(bool succeeded, ICollection<Report> reports = null)
        {
            Succeeded = succeeded;
            Reports = reports;
        }

        public bool Succeeded { get; }

        /// <summary>
        /// Detailed results of a method execution.
        /// </summary>
        public ICollection<Report> Reports { get; }

        /// <summary>
        /// Create instance of <see cref="Result"/>.
        /// </summary>
        public static Result Create(bool succeeded, ICollection<Report> reports = null) => new(succeeded, reports);

        /// <summary>
        /// Create instance of <see cref="Result"/>.
        /// </summary>
        public static Result Create(bool succeeded, Report report = null) => new(succeeded, new List<Report> { report });

        /// <summary>
        /// Create instance of <see cref="Result"/> with property Succeeded "true".
        /// </summary>
        public static Result Success() => new(true);

        /// <summary>
        /// Create instance of <see cref="Result"/> with property Succeeded "true".
        /// </summary>
        public static Result<T> Success<T>(T data) => new(true, data);

        /// <summary>
        /// Create instance of <see cref="Result"/> with property Succeeded "false".
        /// </summary>
        public static Result Failure(ICollection<Report> reports) => new(false, reports);

        /// <summary>
        /// Create instance of <see cref="Result"/> with property Succeeded "false".
        /// </summary>
        public static Result Failure(Report report) => new(false, new List<Report> { report });

        /// <summary>
        /// Create instance of <see cref="Result"/> with property Succeeded "false".
        /// </summary>
        public static Result Failure(ReportCodeType reportCodeType)
        {
            return new Result(false, new List<Report> { Report.Create(reportCodeType) });
        }

        /// <summary>
        /// Create instance of <see cref="Result"/> with property Succeeded "false".
        /// </summary>
        public static Result<T> Failure<T>(ReportCodeType reportCodeType) =>
            new(false, default, new List<Report> { Report.Create(reportCodeType) });

        /// <summary>
        /// Create instance of <see cref="Result"/> with property Succeeded "false".
        /// </summary>
        public static Result<T> Failure<T>(Report report) =>
            new(false, default, new List<Report> { report });
        /// <summary>
        /// Create instance of <see cref="Result"/> with property Succeeded "false".
        /// </summary>
        public static Result<T> Failure<T>(ICollection<Report> reports) =>
            new(false, default, reports);
    }

    /// <summary>
    /// Represents the return of an .
    /// </summary>
    public sealed record Result<T> : Result
    {
        internal Result(bool succeeded, T data, ICollection<Report> reports = null)
            : base(succeeded, reports)
        {
            Data = data;
        }

        /// <summary>
        /// The data itself
        /// </summary>
        public T Data { get; }
    }
}
