namespace Acme.Todoist.Commons.Models.Security
{
    /// <summary>
    /// Represent a user.
    /// </summary>
    public sealed record IdentityUser(
        string Id,
        string Name,
        Role Role,
        IReadOnlyDictionary<string, IReadOnlyCollection<string>> Claims = null) : IIdentityContext
    {
        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public string UserName { get; set; }

        /// <inheritdoc />
        public bool IsAdmin => Role == Role.Admin;

        /// <inheritdoc />
        public bool IsClientApplication => false;

        /// <summary>
        /// Create a new instance de <see cref="IdentityUser"/>.
        /// </summary>
        public static IdentityUser Create(string id, string name, Role role = Role.Anonymous) => new(id, name, role);

        /// <summary>
        /// Cast operator to create a new instance of <see cref="IdentityUser"/> with identifier filled.
        /// </summary>
        public static implicit operator IdentityUser(string id) => Create(id, null);
    }
}
