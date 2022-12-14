using System.Linq;
using Acme.Todoist.Api.Settings;
using Acme.Todoist.Domain.Commons;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Acme.Todoist.Api.Filters
{
    /// <summary>
    /// Filter to ensure that number of records per page is in allowed limits.
    /// </summary>
    public class PagingParametersFilter : ActionFilterAttribute
    {
        private const int DefaultRecordsPerPage = 20;
        private const int MaxRecordsPerPage = 200;


        private readonly PagingSettings _pagingSettings;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="pagingSettings">Settings object where paging is configured.</param>
        public PagingParametersFilter(PagingSettings pagingSettings = null)
        {
            _pagingSettings = pagingSettings ?? new PagingSettings
            {
                DefaultRecordsPerPage = DefaultRecordsPerPage,
                MaxRecordsPerPage = MaxRecordsPerPage
            };
        }

        /// <summary>
        /// Adjusts recordsPerPage value immediately before action is executed.
        /// Values are adjusted only if they are outside the allowed limits.
        /// </summary>
        /// <param name="context">Context with data to be tested and adjusted.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var pagingParametersArg = context.ActionArguments.FirstOrDefault(it => it.Value.GetType() == typeof(PagingParameters));
            if (string.IsNullOrWhiteSpace(pagingParametersArg.Key)) return;

            var (pageNumber, recordsPerPage) = (PagingParameters)pagingParametersArg.Value;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (recordsPerPage < 1 || recordsPerPage > _pagingSettings.MaxRecordsPerPage)
            {
                recordsPerPage = _pagingSettings.DefaultRecordsPerPage;
            }

            context.ActionArguments[pagingParametersArg.Key] = new PagingParameters(pageNumber, recordsPerPage);
        }
    }
}
