﻿using Acme.Todoist.Commons.Models;
using Acme.Todoist.Core.Repositories;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Models.Filters;
using Acme.Todoist.Infrastructure.Models;
using Acme.Todoist.Infrastructure.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Todoist.Core.Features.Queries
{
    public static class SearchProjectsPaginated
    {
        public record Query(
                PagingParameters PagingParameters, 
                OperationContext OperationContext) : PaginatedQuery<PaginatedQueryResult<Project>, Project>(PagingParameters, OperationContext);

        public sealed class QueryHandler : IQueryHandler<Query, PaginatedQueryResult<Project>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public QueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<PaginatedQueryResult<Project>> Handle(Query query, CancellationToken cancellationToken)
            {
                var pagedResult = await _unitOfWork.ProjectRepository.ListPaginatedByFilterAsync(new ProjectFilter(), query.PagingParameters, cancellationToken);

                return QueryResult.Ok(pagedResult.Results, query.PagingParameters, pagedResult);
            }
        }
    }
}
