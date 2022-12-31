using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Data.Contexts;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Models.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Data.Repositories;

public sealed class ProjectRepository : Repository, IProjectRepository
{
    public ProjectRepository(MainContext context) : base(context) { }

    public Task<Project> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task<PaginatedResult<Project>> ListPaginatedByFilterAsync(ProjectFilter filter, PagingParameters pagingParameters,
        CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task CreateAsync(Project project, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteAsync(Project todo, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}