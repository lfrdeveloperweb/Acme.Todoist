using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Core.Security
{
    public interface ISecurityService
    {
        Task<SignInResult> CheckPasswordAsync(User user, string password);
    }
}
