using Acme.Todoist.Application.DataContracts.Responses;
using Acme.Todoist.Core.Features.Queries;
using Acme.Todoist.Domain.Models;
using Acme.Todoist.Infrastructure.Models;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.DataContracts.Requests;
using Acme.Todoist.Core.Features.Commands;
using Acme.Todoist.Commons.Models;
using Acme.Todoist.Infrastructure.Queries;

namespace Acme.Todoist.Application.Services
{
    public sealed class TodoAppService : AppServiceBase
    {
        public TodoAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

        public async Task<Response<TodoResponseData>> GetAsync(string id, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var query = new GetTodoDetails.Query(id, operationContext);
            var queryResult = await Dispatcher.Send(query, cancellationToken).ConfigureAwait(false);

            return Response.From<Todo, TodoResponseData>(queryResult, Mapper);
        }

        public async Task<PaginatedResponse<TodoResponseData>> SearchAsync(PagingParameters pagingParameters, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var query = new SearchTodosPaginated.Query(pagingParameters, operationContext);
            PaginatedQueryResult<Todo> result = await Dispatcher.Send(query, cancellationToken).ConfigureAwait(false);

            return Response.From<Todo, TodoResponseData>(result, Mapper);
        }

        public async ValueTask<Response<TodoResponseData>> CreateAsync(TodoForCreationRequest request, OperationContext operationContext, CancellationToken cancellationToken)
        {
            var command = new CreateTodo.Command(
                request.Title,
                request.Description,
                request.ProjectId,
                request.DueDate,
                request.Priority,
                request.Tags,
                operationContext);

            var result = await Dispatcher.Send(command, cancellationToken);

            return Response.From<Todo, TodoResponseData>(result, Mapper);
        }

        public ValueTask<Response> DeleteAsync(string id, OperationContext operationContext, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
