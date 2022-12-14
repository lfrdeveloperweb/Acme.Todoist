using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Models.Filters;

namespace Acme.Todoist.Application.Repositories;

public interface IProjectRepository
{
    /// <summary>
    /// Retrieves an <see cref="Project"/> by its identifier.
    /// </summary>
    Task<Project> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<PaginatedResult<Project>> ListPaginatedByFilterAsync(ProjectFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

    Task CreateAsync(Project project, CancellationToken cancellationToken);

    Task DeleteAsync(Project todo, CancellationToken cancellationToken);
}