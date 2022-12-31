using Acme.Todoist.Domain.Models;

namespace Acme.Todoist.Domain.Security
{
    /// <summary>
    /// Represent a membership.
    /// </summary>
    public sealed record Membership
    {
        public required string Id { get; init; }

        public required string Name { get; init; }

        public static Membership From(IIdentityContext identityContext) => new()
        {
            Id = identityContext.Id,
            Name = identityContext.Name
        };
         
        public static Membership From(User user) => new()
        {
            Id = user.Id,
            Name = user.Name
        };
    }
}
