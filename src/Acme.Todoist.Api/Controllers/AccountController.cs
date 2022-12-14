using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Api.Services;
using Acme.Todoist.Application.DataContracts.Requests;
using Acme.Todoist.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Todoist.Api.Controllers;

[Route("accounts")]
public sealed class AccountController : ApiController
{
    private readonly AccountAppService _service;

    public AccountController(AccountAppService service, IOperationContextManager operationContextManager)
        : base(operationContextManager)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Get([FromBody] LoginRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.LoginAsync(request, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));
}