using Acme.Todoist.Application.Core.Queries;
using Acme.Todoist.Application.Features.Todos;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Models;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.DataContracts.Requests;
using Acme.Todoist.Application.DataContracts.Responses;

namespace Acme.Todoist.Application.Services;

public sealed class TodoAppService : AppServiceBase
{
    public TodoAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

    public async Task<Response<TodoResponseData>> GetAsync(string id, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var query = new GetTodoDetails.Query(id, operationContext);
        var queryResult = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

        return Response.From<Todo, TodoResponseData>(queryResult, Mapper);
    }

    public async Task<PaginatedResponse<TodoResponseData>> SearchAsync(PagingParameters pagingParameters, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var query = new SearchTodosPaginated.Query(pagingParameters, operationContext);
        var result = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

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
            request.Labels,
            operationContext);

        var result = await Sender.Send(command, cancellationToken);

        return Response.From<Todo, TodoResponseData>(result, Mapper);
    }

    public async ValueTask<Response<TodoResponseData>> UpdateAsync(string id, TodoForUpdateRequest request, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var command = new UpdateTodo.Command(
            id,
            request.Title,
            request.Description,
            request.ProjectId,
            request.DueDate,
            request.Priority,
            request.Labels,
            operationContext);

        var result = await Sender.Send(command, cancellationToken);

        return Response.From<Todo, TodoResponseData>(result, Mapper);
    }

    public async ValueTask<Response> CloneAsync(string id, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var command = new CloneTodo.Command(id, operationContext);
        var result = await Sender.Send(command, cancellationToken);

        return Response.From(result);
    }

    public ValueTask<Response> DeleteAsync(string id, OperationContext operationContext, CancellationToken cancellationToken) => throw new System.NotImplementedException();

    public async Task<PaginatedResponse<TodoCommentResponseData>> SearchCommentsAsync(string todoId, PagingParameters pagingParameters, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var query = new SearchTodoCommentsPaginated.Query(todoId, pagingParameters, operationContext);
        PaginatedQueryResult<TodoComment> result = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

        return Response.From<TodoComment, TodoCommentResponseData>(result, Mapper);
    }

    public async ValueTask<Response<TodoCommentResponseData>> CreateCommentAsync(string todoId, TodoCommentForCreationRequest request, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var command = new CreateTodoComment.Command(
            todoId,
            request.Description,
            operationContext);

        var result = await Sender.Send(command, cancellationToken);

        return Response.From<TodoComment, TodoCommentResponseData>(result, Mapper);
    }

    public async ValueTask<Response> DeleteCommentAsync(string id, string todoId, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var command = new DeleteTodoComment.Command(id, todoId, operationContext);
        var result = await Sender.Send(command, cancellationToken);

        return Response.From(result);
    }
}