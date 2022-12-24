using Acme.Todoist.Domain.Security;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Acme.Todoist.Application.Factories
{
    public class IdentityContextFactory : IIdentityContextFactory
    {
        public IIdentityContext Create(ClaimsPrincipal principal)
        {
            if (!principal.Identity?.IsAuthenticated ?? false)
            {
                return default;
            }

            IReadOnlyDictionary<string, IReadOnlyCollection<string>> claims = principal.Claims
                .GroupBy(claim => claim.Type)
                .ToDictionary(claim => claim.Key, claim => (IReadOnlyCollection<string>)claim.Select(it => it.Value).ToList(), StringComparer.OrdinalIgnoreCase);

            return new IdentityUser(
                Id: principal.FindFirstValue(JwtClaimTypes.Subject), 
                Name: principal.FindFirstValue(JwtClaimTypes.Name), 
                Role: Enum.Parse<Role>(principal.FindFirstValue(JwtClaimTypes.Role)), 
                claims);
        }
    }
}
