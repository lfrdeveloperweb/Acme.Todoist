using Acme.Todoist.Domain.Security;
using Microsoft.AspNetCore.Authorization;

namespace Acme.Todoist.Infrastructure.Security
{
    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(PermissionType permissionType)
        {
            PermissionType = permissionType;
        }

        public PermissionType PermissionType { get; }
    }
}
