using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Acme.Todoist.Application.DataContracts
{
    namespace Requests
    {
        public sealed record TodoForCreationRequest(
            [Required] string Title,
            string Description,
            string ProjectId,
            DateTime? DueDate,
            [Required] int Priority,
            ICollection<string> Labels);

        public sealed record TodoForUpdateRequest(
            [Required] string Title,
            string Description,
            string ProjectId,
            DateTime? DueDate,
            [Required] int Priority,
            ICollection<string> Labels);

        public sealed record TodoCommentForCreationRequest(
            string Description);
    }

    namespace Responses
    {
        public sealed record TodoResponseData(
            string Id,
            string Title,
            string Description,
            DateTime? DueDate,
            int Priority,
            ICollection<string> Labels,
            DateTimeOffset? CompletedAt,
            DateTimeOffset CreatedAt,
            IdentityNamedResponse CreatedBy,
            DateTimeOffset? UpdatedAt);

        public sealed record TodoCommentResponseData(
            string Id,
            string Description,
            DateTimeOffset CreatedAt);
    }
}
