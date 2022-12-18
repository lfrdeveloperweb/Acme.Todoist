using Acme.Todoist.Domain.Security;
using System.Security.Claims;

namespace Acme.Todoist.Application.Factories
{
    public interface IIdentityContextFactory
    {
        IIdentityContext Create(ClaimsPrincipal principal);
    }
}
