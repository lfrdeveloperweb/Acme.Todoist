using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acme.Todoist.Commons.Models;

namespace Acme.Todoist.Infrastructure.Commands
{
    /// <summary>
    /// The result of running a validator.
    /// </summary>
    public sealed record CommandValidationResult()
    {
        public CommandValidationResult(ICollection<Report> reports) : this()
        {
            Reports = reports ?? new List<Report>();
        }

        /// <summary>
        /// Whether validation succeeded.
        /// </summary>
        public bool IsValid { get; private init; }

        /// <summary>
        /// A collection of <see cref="Report"/>.
        /// </summary>
        public ICollection<Report> Reports { get; } = new Collection<Report>();

        /// <summary>
        /// 
        /// </summary>
        public static CommandValidationResult Succeeded => new() { IsValid = true };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reports"></param>
        /// <returns></returns>
        public static CommandValidationResult Failed(ICollection<Report> reports) => new(reports) { IsValid = false };
    }
}
