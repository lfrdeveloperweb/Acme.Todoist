using System.Collections.Generic;
using System.Linq;

namespace Acme.Todoist.Domain.Security
{
    /// <summary>
    /// Represent a user.
    /// </summary>
    public interface IIdentityContext
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Name of user.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Role of user.
        /// </summary>
        Role Role { get; }

        /// <summary>
        /// Flag that indicate if user is admin.
        /// </summary>
        bool IsAdmin { get; }

        /// <summary>
        /// Flag that indicate if user is a client application.
        /// </summary>
        bool IsClientApplication { get; }

        /// <summary>
        /// Claims.
        /// </summary>
        IReadOnlyDictionary<string, IReadOnlyCollection<string>> Claims { get; }

        /// <summary>
        /// Get first or default value by claim type from <see cref="Claims"/>.
        /// </summary>
        /// <param name="claimType"></param>
        /// <returns></returns>
        string GetFirstOrDefaultClaimValue(string claimType) => Claims.TryGetValue(claimType, out var values) ? values.FirstOrDefault() : null;
    }
}
