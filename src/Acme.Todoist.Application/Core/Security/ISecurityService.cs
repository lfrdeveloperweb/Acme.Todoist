using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Core.Security
{
    public interface ISecurityService
    {
        Task<Result> CheckPasswordAsync(User user, string password);
    }
}
