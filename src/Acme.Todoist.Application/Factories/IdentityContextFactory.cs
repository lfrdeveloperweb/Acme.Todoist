using Acme.Todoist.Domain.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Acme.Todoist.Application.Factories
{
    public class IdentityContextFactory : IIdentityContextFactory
    {
        public IIdentityContext Create(ClaimsPrincipal principal)
        {
            if (!principal?.Identity?.IsAuthenticated ?? false)
            {
                return default;
            }

            var subjectId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var name = principal.FindFirstValue(JwtRegisteredClaimNames.Name);

            if (!Enum.TryParse(principal.FindFirstValue("role"), true, out Role role))
            {
                // Warning
            }

            IReadOnlyDictionary<string, IReadOnlyCollection<string>> claims = principal.Claims
                .GroupBy(claim => claim.Type)
                .ToDictionary(claim => claim.Key, claim => (IReadOnlyCollection<string>)claim.Select(it => it.Value).ToList(), StringComparer.OrdinalIgnoreCase);

            return new IdentityUser(subjectId, name, role, claims);
        }
    }
}
