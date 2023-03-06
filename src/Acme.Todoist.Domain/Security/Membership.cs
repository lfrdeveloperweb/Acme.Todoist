using Acme.Todoist.Domain.Models;

namespace Acme.Todoist.Domain.Security
{
    /// <summary>
    /// Represent a membership.
    /// </summary>
    public sealed record Membership(string Id, string Name)
    {
        public static Membership From(IIdentityContext identityContext) => new(identityContext.Id, identityContext.Name);
         
        public static Membership From(User user) => new(user.Id, user.Name);
    }
}
