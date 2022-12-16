using Acme.Todoist.Application.Core.Queries;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Features.Projects
{
    public sealed class GetProjectDetails
    {
        public sealed record Query(string Id, OperationContext OperationContext) : Query<QueryResult<Project>>(OperationContext);

        public sealed class QueryHandler : IQueryHandler<Query, QueryResult<Project>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public QueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<QueryResult<Project>> Handle(Query query, CancellationToken cancellationToken) =>
                QueryResult.OkOrNotFound(await _unitOfWork.ProjectRepository.GetByIdAsync(query.Id, cancellationToken));
        }
    }
}
