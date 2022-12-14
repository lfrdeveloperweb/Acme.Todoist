using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Acme.Todoist.Api.Filters
{
    /// <summary>
    /// Handler general exception.
    /// </summary>
    public sealed class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> log)
        {
            _logger = log;
        }

        /// <summary>
        /// Handler global exception.
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            var report = Report.Create(ReportCodeType.UnexpectedError);
            // var response = new OperationResponse(OperationState.InternalServerError, report);

            // Log error because if it reached this code it's not an unexpected error from Api.
            _logger.LogError(context.Exception, report.Message);

            // Build result and return
            context.Result = new JsonResult(report) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
