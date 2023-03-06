using System.Collections.Generic;

namespace Acme.Todoist.Domain.Security
{
    /// <summary>
    /// Represent a user.
    /// </summary>
    public sealed record IdentityUser(
        string Id,
        string Name,
        Role Role,
        bool IsAuthenticated = false,
        IReadOnlyDictionary<string, IReadOnlyCollection<string>> Claims = null) : IIdentityContext
    {
        public static readonly IdentityUser Anonymous = new(
            Id: null,
            Name: "Anonymous",
            Role: Role.Anonymous,
            IsAuthenticated: false);

        /// <inheritdoc />
        public bool IsAdmin => Role == Role.Admin;

        /// <inheritdoc />
        public bool IsClientApplication => false;
    }
}
