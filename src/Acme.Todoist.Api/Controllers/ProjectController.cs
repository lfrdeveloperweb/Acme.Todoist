using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Api.Services;
using Acme.Todoist.Application.DataContracts.Requests;
using Acme.Todoist.Application.Services;
using Acme.Todoist.Domain.Commons;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Todoist.Api.Controllers;

[Route("projects")]
public sealed class ProjectController : ApiController
{
    private readonly ProjectAppService _service;

    public ProjectController(ProjectAppService service, IOperationContextManager operationContextManager)
        : base(operationContextManager)
    {
        _service = service;
    }

    /// <summary>
    /// Get task by id.
    /// </summary>
    //[Permission(PermissionType.OrderRead)]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.GetAsync(id, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Search task by filter.
    /// </summary>
    //[ResourceAuthorization(PermissionType.OrderFull, PermissionType.OrderRead)]
    [HttpPost("search")]
    public async Task<IActionResult> Search(PagingParameters pagingParameters, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.SearchAsync(pagingParameters, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProjectForCreationRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.CreateAsync(request, base.OperationContextManager.GetContext(), cancellationToken));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.DeleteAsync(id, base.OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));
}