namespace Acme.Todoist.Infrastructure.Security
{
    /// <summary>
    /// Settings for user accounts.
    /// </summary>
    public sealed class AccountSettings
    {
        /// <summary>
        /// Gets or sets the number of failed access attempts allowed before a user is locked out, assuming lock out is enabled.
        /// </summary>
        /// <value>
        /// The number of failed access attempts allowed before a user is locked out, if lockout is enabled.
        /// </value>
        public int MaxFailedAccessAttempts { get; set; }
    }
}
