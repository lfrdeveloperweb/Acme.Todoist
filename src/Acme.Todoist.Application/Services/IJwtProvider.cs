using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Security;

namespace Acme.Todoist.Application.Services
{
    /// <summary>
    /// Represents the JWT provider interface.
    /// </summary>
    public interface IJwtProvider
    {
        /// <summary>
        /// Creates the JWT for the specified <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The JWT for the specified user.</returns>
        JwtToken Generate(User user);
    }
}
