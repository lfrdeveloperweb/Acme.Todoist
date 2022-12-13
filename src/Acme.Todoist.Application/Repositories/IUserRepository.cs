using Acme.Todoist.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

    Task CreateAsync(User user, CancellationToken cancellationToken);

    Task UpdateAsync(User user, CancellationToken cancellationToken);
}