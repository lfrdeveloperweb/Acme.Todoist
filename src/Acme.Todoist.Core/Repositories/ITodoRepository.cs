using Acme.Todoist.Commons.Models;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Models.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Core.Repositories
{
    public interface ITodoRepository
    { /// <summary>
        /// Retrieves an <see cref="Todo"/> by its identifier.
        /// </summary>
        Task<Todo> GetByIdAsync(string id, CancellationToken cancellationToken);

        Task<PaginatedResult<Todo>> ListPaginatedByFilterAsync(TodoFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

        Task CreateAsync(Todo todo, CancellationToken cancellationToken);

        Task DeleteAsync(Todo todo, CancellationToken cancellationToken);
    }
}
