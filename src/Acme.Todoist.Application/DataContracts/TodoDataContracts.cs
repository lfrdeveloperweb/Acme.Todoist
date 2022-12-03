using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Acme.Todoist.Application.DataContracts
{
    namespace Requests
    {
        public sealed record TodoForCreationRequest(
            [Required] string Title,
            string ProjectId,
            DateTime? DueDate,
            [Required] int Priority,
            ICollection<string> Tags);
    }

    namespace Responses
    {
        public sealed record TodoResponseData(
            string Id,
            string Title,
            DateTime? DueDate,
            int Priority,
            DateTimeOffset? CompletedAt,
            DateTimeOffset CreatedAt);
    }
}
