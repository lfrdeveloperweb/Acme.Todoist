using System;
using Acme.Todoist.Commons.Models.Security;

namespace Acme.Todoist.Domain.Models
{
    public sealed class TodoComment
    {
        public int Id { get; set; }

        public string TodoId { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Identifier of user that created the record.
        /// </summary>
        public Membership CreatedBy { get; set; }

        /// <summary>
        /// Date and time of record creation.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

    }
}