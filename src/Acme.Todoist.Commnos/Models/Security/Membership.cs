﻿namespace Acme.Todoist.Commons.Models.Security
{
    /// <summary>
    /// Represent a membership.
    /// </summary>
    public sealed record Membership
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public static Membership From(IIdentityContext identityContext) => new()
        {
            Id = identityContext.Id,
            Name = identityContext.Name
        };
    }
}
