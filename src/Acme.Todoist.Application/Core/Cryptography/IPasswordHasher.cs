namespace Acme.Todoist.Application.Core.Cryptography
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Returns a hashed representation of the supplied <paramref name="password"/>.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password"/>.</returns>
        string HashPassword(string password);

        /// <summary>
        /// Returns a <see cref="bool"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="hashedPassword">The hash value for a user's stored password.</param>
        /// <param name="password">The password supplied for comparison.</param>
        /// <returns>A <see cref="bool"/> indicating the result of a password hash comparison.</returns>
        /// <remarks>Implementations of this method should be time consistent.</remarks>
        bool VerifyHashedPassword(string hashedPassword, string password);
    }
}
