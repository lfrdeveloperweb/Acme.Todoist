using Acme.Todoist.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Services
{
    public interface IIdentityService
    {
        IIdentityContext GetIdentity();
    }
}
