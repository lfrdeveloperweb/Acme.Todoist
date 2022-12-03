using Acme.Todoist.Api.Services;
using Acme.Todoist.Application.DataContracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Todoist.Api.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected ApiController(IOperationContextManager operationContextManager = null)
        {
            OperationContextManager = operationContextManager;
        }

        protected IOperationContextManager OperationContextManager { get; }

        [NonAction]
        protected IActionResult BuildActionResult<TResponse>(TResponse response) where TResponse : Response =>
            response.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(response),
                StatusCodes.Status201Created => StatusCode(response.StatusCode, response),
                StatusCodes.Status202Accepted => Accepted(),
                StatusCodes.Status204NoContent => NoContent(),
                StatusCodes.Status206PartialContent => StatusCode(response.StatusCode, response),
                StatusCodes.Status401Unauthorized => Unauthorized(response),
                StatusCodes.Status403Forbidden => Forbid(),
                StatusCodes.Status404NotFound => NotFound(),
                StatusCodes.Status422UnprocessableEntity => StatusCode(response.StatusCode, response),
                StatusCodes.Status503ServiceUnavailable => StatusCode(response.StatusCode, response),
                _ => StatusCode(response.StatusCode, response)
            };
    }
}
