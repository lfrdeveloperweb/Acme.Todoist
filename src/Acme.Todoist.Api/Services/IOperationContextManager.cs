using Acme.Todoist.Domain.Commons;

namespace Acme.Todoist.Api.Services;

public interface IOperationContextManager
{
    /// <summary>
    /// Retrieve context of a request.
    /// </summary>
    OperationContext GetContext();
}