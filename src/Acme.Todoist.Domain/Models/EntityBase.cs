using System;
using Acme.Todoist.Commons.Models.Security;

namespace Acme.Todoist.Domain.Models
{
    public class EntityBase
    {
        /// <summary>
        /// Identifier of user that created the record.
        /// </summary>
        public Membership CreatedBy { get; set; }

        /// <summary>
        /// Date and time of record creation.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Identifier of user that done last modification of the record.
        /// </summary>
        public Membership UpdatedBy { get; set; }

        /// <summary>
        /// Date and time of last modification of the record.
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
