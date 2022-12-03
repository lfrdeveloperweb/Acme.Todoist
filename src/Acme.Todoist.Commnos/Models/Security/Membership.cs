namespace Acme.Todoist.Commons.Models.Security
{
    /// <summary>
    /// Represent a membership.
    /// </summary>
    public sealed record Membership(string MembershipId, string Name)
    {
        public static Membership From(IIdentityContext identityContext) => new(identityContext.Id, identityContext.Name);
    }
}
