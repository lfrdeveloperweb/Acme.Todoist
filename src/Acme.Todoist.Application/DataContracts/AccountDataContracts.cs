using System.ComponentModel.DataAnnotations;

namespace Acme.Todoist.Application.DataContracts
{
    namespace Requests
    {
        public sealed record LoginRequest(
            [Required] string Email,
            [Required] string Password);
    }

    namespace Responses
    {
        public sealed record TokenResponseData(
            string Token,
            string TokenType,
            int ExpiresIn);
    }
}
