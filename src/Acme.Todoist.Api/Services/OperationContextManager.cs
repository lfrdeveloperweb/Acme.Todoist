using Acme.Todoist.Api.Constants;
using Acme.Todoist.Commons.Models.Security;
using Acme.Todoist.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Acme.Todoist.Api.Services
{
    public interface IOperationContextManager
    {
        /// <summary>
        /// Retrieve context of a request.
        /// </summary>
        OperationContext GetContext();
    }

    /// <summary>
    /// Request manager.
    /// </summary>
    public sealed class OperationContextManager : IOperationContextManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        // private readonly IIdentityContextFactory _identityContextFactory;

        //public OperationContextManager(IHttpContextAccessor httpContextAccessor, IIdentityContextFactory identityContextFactory)
        public OperationContextManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            // _identityContextFactory = identityContextFactory;
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
                //Identity: _identityContextFactory.Create(context.User),
                Identity: new IdentityUser("anonymous", nameof(Role.Anonymous), Role.Anonymous),
                IsAuthenticated: context.User.Identity.IsAuthenticated,
                InternalSourceIp: connection.LocalIpAddress?.ToString(),
                ExternalSourceIp: connection.RemoteIpAddress?.ToString());
        }
    }
}
