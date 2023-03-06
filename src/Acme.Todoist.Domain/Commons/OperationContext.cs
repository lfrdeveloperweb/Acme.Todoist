using Acme.Todoist.Domain.Security;

namespace Acme.Todoist.Domain.Commons
{
    /// <summary>
    /// Represent the context of a request.
    /// </summary>
    public record OperationContext(
        string CorrelationId,
        IIdentityContext Identity,
        string InternalSourceIp = null,
        string ExternalSourceIp = null);
}
