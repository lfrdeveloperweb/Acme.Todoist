using System;
using System.ComponentModel.DataAnnotations;
using Acme.Todoist.Domain.Models;

namespace Acme.Todoist.Application.DataContracts
{
    namespace Requests
    {
        public sealed record RegisterAccountRequest(
            [Required] string Name,
            DateOnly? BirthDate,
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
            string Token,
            string TokenType,
            int ExpiresIn);

        public sealed record UserResponseData(
            string Id,
            string Name,
            DateOnly? BirthDate,
            string Email,
            string PhoneNumber,
            Role Role,
            bool IsLocked,
            int LoginCount,
            DateTimeOffset? LastLoginAt);
    }
}
