using Acme.Todoist.Application.Services;
using Acme.Todoist.Domain.Security;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Acme.Todoist.Api.Services;

public sealed class IdentityService : IIdentityService
{
    private readonly Lazy<IIdentityContext> _identityContext;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _identityContext = new Lazy<IIdentityContext>(() =>
        {
            var principal = httpContextAccessor.HttpContext.User;
            if (!principal.Identity?.IsAuthenticated ?? false)
            {
                return IdentityUser.Anonymous;
            }

            IReadOnlyDictionary<string, IReadOnlyCollection<string>> claims = principal.Claims
                .GroupBy(claim => claim.Type)
                .ToDictionary(claim => claim.Key, claim => (IReadOnlyCollection<string>)claim.Select(it => it.Value).ToList(),
                    StringComparer.OrdinalIgnoreCase);

            return new IdentityUser(
                Id: principal.FindFirstValue(JwtClaimTypes.Subject),
                Name: principal.FindFirstValue(JwtClaimTypes.Name),
                Role: Enum.Parse<Role>(principal.FindFirstValue(JwtClaimTypes.Role)),
                IsAuthenticated: true,
                claims);
        });
    }

    public IIdentityContext GetIdentity() => _identityContext.Value;
}