namespace Acme.Todoist.Infrastructure.Security
{
    /// <summary>
    /// Represents the JWT configuration settings.
    /// </summary>
    public sealed record JwtSettings
    {
        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        public string Issuer { get; init; }

        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        public string Audience { get; init; }

        /// <summary>
        /// Gets or sets the security key.
        /// </summary>
        public string SecurityKey { get; init; }

        /// <summary>
        /// Gets or sets the token expiration time in minutes.
        /// </summary>
        public int TokenExpirationInMinutes { get; init; }
    }
}
