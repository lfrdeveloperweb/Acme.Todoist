using Acme.Todoist.Domain.Security;

namespace Acme.Todoist.Application.Services
{
    public interface IIdentityService
    {
        IIdentityContext GetIdentity();
    }
}
