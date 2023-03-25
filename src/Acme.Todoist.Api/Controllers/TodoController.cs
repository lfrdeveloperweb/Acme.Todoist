using System.Net.Http;
using Acme.Todoist.Api.Services;
using Acme.Todoist.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Application.DataContracts.Requests;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Security;
using Acme.Todoist.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;

namespace Acme.Todoist.Api.Controllers
{
    [Authorize]
    [Route("todos")]
    public sealed class TodoController : ApiController
    {
        private readonly TodoAppService _service;

        public TodoController(TodoAppService service, IOperationContextManager operationContextManager)
            : base(operationContextManager)
        {
            _service = service;
        }

        /// <summary>
        /// Get task by id.
        /// </summary>
        [HasPermission(PermissionType.TodoRead)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .GetAsync(id, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        /// <summary>
        /// Search task by filter.
        /// </summary>
        //[ResourceAuthorization(PermissionType.TodoRead)]
        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<IActionResult> Search(PagingParameters pagingParameters, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .SearchAsync(pagingParameters, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoForCreationRequest request, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .CreateAsync(request, base.OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpPost("{id}/clone")]
        public async Task<IActionResult> Clone(string id, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .CloneAsync(id, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TodoForUpdateRequest request, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .UpdateAsync(id, request, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .DeleteAsync(id, base.OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));


        /// <summary>
        /// Search comments by filter.
        /// </summary>
        [HttpPost("{todoId}/comments/search")]
        public async Task<IActionResult> SearchComments(string todoId, PagingParameters pagingParameters, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .SearchCommentsAsync(todoId, pagingParameters, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpPost("{todoId}/comments")]
        public async Task<IActionResult> PostComment(string todoId, [FromBody] TodoCommentForCreationRequest request, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .CreateCommentAsync(todoId, request, base.OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpDelete("{todoId}/comments/{id}")]
        public async Task<IActionResult> DeleteComment(string id, string todoId, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .DeleteCommentAsync(id, todoId, base.OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));
    }
}
