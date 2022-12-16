using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.Core.Queries;
using Acme.Todoist.Application.Repositories;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;

namespace Acme.Todoist.Application.Features.Accounts
{
    public sealed class GetUserDetails
    {
        public sealed record Query(string Id, OperationContext OperationContext) : Query<QueryResult<User>>(OperationContext);

        public sealed class QueryHandler : IQueryHandler<Query, QueryResult<User>>
        {
            private readonly IUserRepository _userRepository;

            public QueryHandler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<QueryResult<User>> Handle(Query query, CancellationToken cancellationToken) =>
                QueryResult.OkOrNotFound(await _userRepository.GetByIdAsync(query.Id, cancellationToken));
        }
    }
}
