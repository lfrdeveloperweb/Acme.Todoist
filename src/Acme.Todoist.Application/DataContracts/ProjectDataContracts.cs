using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.Todoist.Application.DataContracts
{
    namespace Requests
    {
        public sealed record ProjectForCreationRequest(
            [Required] string Title,
            string Color);
    }

    namespace Responses
    {
        public sealed record ProjectResponseData(
            string Title,
            string Color,
            DateTimeOffset CreatedAt);
    }
}
