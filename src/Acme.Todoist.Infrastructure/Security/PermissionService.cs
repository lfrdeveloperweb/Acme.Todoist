using Acme.Todoist.Domain.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.Todoist.Infrastructure.Security
{
    public sealed class PermissionService : IPermissionService
    {
        public PermissionService()
        {
            
        }

        public Task<HashSet<PermissionType>> ListPermissionsAsync(string userId)
        {
            return Task.FromResult(new HashSet<PermissionType>
            {
                PermissionType.TodoRead,
                PermissionType.TodoCreate,
                PermissionType.TodoUpdate,
                PermissionType.TodoDelete,
            });
        }
    }
}
