using Acme.Todoist.Api.Constants;
using Acme.Todoist.Application.Factories;
using Acme.Todoist.Domain.Commons;
using Microsoft.AspNetCore.Http;

namespace Acme.Todoist.Api.Services
{
    /// <summary>
    /// Request manager.
    /// </summary>
    public sealed class OperationContextManager : IOperationContextManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityContextFactory _identityContextFactory;

        public OperationContextManager(IHttpContextAccessor httpContextAccessor, IIdentityContextFactory identityContextFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityContextFactory = identityContextFactory;
        }

        /// <summary>
        /// Retrieve context of a request.
        /// </summary>
        public OperationContext GetContext()
        {
            var context = _httpContextAccessor.HttpContext;
            var connection = context.Request.HttpContext.Connection;

            return new OperationContext(
                CorrelationId: context.Request.Headers[ApplicationHeaderNames.RequestId],
                Identity: _identityContextFactory.Create(context.User),
                IsAuthenticated: context.User.Identity.IsAuthenticated,
                InternalSourceIp: connection.LocalIpAddress?.ToString(),
                ExternalSourceIp: connection.RemoteIpAddress?.ToString());
        }
    }
}
