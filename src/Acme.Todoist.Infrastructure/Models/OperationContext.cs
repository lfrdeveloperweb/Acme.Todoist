using Acme.Todoist.Commons.Models.Security;

namespace Acme.Todoist.Infrastructure.Models
{
    /// <summary>
    /// Represent the context of a request.
    /// </summary>
    public record OperationContext(
        string CorrelationId,
        IIdentityContext Identity,
        bool IsAuthenticated,
        string InternalSourceIp = null,
        string ExternalSourceIp = null);
}
