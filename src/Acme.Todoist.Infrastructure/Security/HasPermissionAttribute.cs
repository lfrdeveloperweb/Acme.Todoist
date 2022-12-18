using Acme.Todoist.Domain.Security;
using Microsoft.AspNetCore.Authorization;

namespace Acme.Todoist.Infrastructure.Security;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(PermissionType permissionType) : base(permissionType.ToString()) { }
}