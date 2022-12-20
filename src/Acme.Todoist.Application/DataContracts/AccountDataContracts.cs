using Acme.Todoist.Domain.Security;
using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.Todoist.Application.DataContracts
{
    namespace Requests
    {
        public sealed record RegisterAccountRequest(
            [Required] string Name,
            DateTime? BirthDate,
            [Required] string Email,
            [Required] string PhoneNumber,
            [Required] string Password,
            [Required] string ConfirmPassword);

        public sealed record LoginRequest(
            [Required] string Email,
            [Required] string Password);
    }

    namespace Responses
    {
        public sealed record JwtTokenResponseData(
            string AccessToken,
            string TokenType,
            int ExpiresIn);

        public sealed record UserResponseData(
            string Id,
            string Name,
            DateOnly? BirthDate,
            string Email,
            bool EmailConfirmed,
            string PhoneNumber,
            bool PhoneNumberConfirmed,
            Role Role,
            bool IsLocked,
            int LoginCount,
            DateTimeOffset? LastLoginAt);
    }
}
