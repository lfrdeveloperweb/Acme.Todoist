using Acme.Todoist.Application.Core.Queries;
using Acme.Todoist.Application.Features.Projects;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.DataContracts.Requests;
using Acme.Todoist.Application.DataContracts.Responses;

namespace Acme.Todoist.Application.Services
{
    public sealed class ProjectAppService : AppServiceBase
    {
        public ProjectAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

        public async Task<Response<ProjectResponseData>> GetAsync(string id, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var query = new GetProjectDetails.Query(id, operationContext);
            var queryResult = await Dispatcher.Send(query, cancellationToken).ConfigureAwait(false);

            return Response.From<Project, ProjectResponseData>(queryResult, Mapper);
        }

        public async Task<PaginatedResponse<ProjectResponseData>> SearchAsync(PagingParameters pagingParameters, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var query = new SearchProjectsPaginated.Query(pagingParameters, operationContext);
            PaginatedQueryResult<Project> result = await Dispatcher.Send(query, cancellationToken).ConfigureAwait(false);

            return Response.From<Project, ProjectResponseData>(result, Mapper);
        }

        public async ValueTask<Response<ProjectResponseData>> CreateAsync(ProjectForCreationRequest request, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var command = new CreateProject.Command(
                request.Title,
                request.Color,
                operationContext);

            var result = await Dispatcher.Send(command, cancellationToken);

            return Response.From<Project, ProjectResponseData>(result, Mapper);
        }

        public ValueTask<Response> DeleteAsync(string id, OperationContext operationContext, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
