using Acme.Todoist.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<User> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken);

    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);
    
    Task<bool> ExistByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken);

    Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> ExistByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);

    Task<bool> ExistByUserNameAsync(string userName, CancellationToken cancellationToken);

    Task CreateAsync(User user, CancellationToken cancellationToken);

    Task UpdateAsync(User user, CancellationToken cancellationToken);

    Task ChangePasswordAsync(User user, CancellationToken cancellationToken);
}