using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.Todoist.Domain.Security;

namespace Acme.Todoist.Infrastructure.Security;

public interface IPermissionService
{
    Task<HashSet<PermissionType>> ListPermissionsAsync(string userId);
}