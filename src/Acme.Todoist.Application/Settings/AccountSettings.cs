namespace Acme.Todoist.Application.Settings
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
        public int MaxFailedAccessAttempts { get; init; }

        /// <summary>
        /// Lifetime in minutes of token to confirm phone number.
        /// </summary>
        public int PhoneNumberConfirmationTokenExpirationInMinutes { get; init; }

        /// <summary>
        /// Lifetime in minutes of token to confirm email address.
        /// </summary>
        public int EmailConfirmationTokenExpirationInMinutes { get; init; }

        /// <summary>
        /// Lifetime in minutes of token to reset password.
        /// </summary>
        public int PasswordResetTokenExpirationInMinutes { get; init; }
    }
}
