using Acme.Todoist.Domain.Resources;

namespace Acme.Todoist.Domain.Commons
{
    /// <summary>
    /// Represents a error or warning.
    /// </summary>
    /// <param name="Code">Error code.</param>
    /// <param name="Field"> Name's field.</param>
    /// <param name="Message"Description of error or warning.></param>
    public sealed record Report(int Code, string Field, string Message)
    {
        /// <summary>
        /// Create a instance of <see cref="Report"/>.
        /// </summary>
        public static Report Create(int code, string message) => new Report(code, null, message);

        /// <summary>
        /// Create a instance of <see cref="Report"/>.
        /// </summary>
        public static Report Create(int code, string field, string message) => new Report(code, field, message);

        /// <summary>
        /// Create a instance of <see cref="Report"/> from <see cref="ReportCodeType"/>.
        /// </summary>
        /// <param name="reportCodeType"></param>
        /// <returns>Instance of <see cref="Report"/> with <see cref="Code"/> and <see cref="Message"/> filled from Resource.</returns>
        public static Report Create(ReportCodeType reportCodeType) => Create((int)reportCodeType, ReportCodeMessage.GetMessage(reportCodeType));

        /// <summary>
        /// Create a instance of <see cref="Report"/> from <see cref="ReportCodeType"/>.
        /// </summary>
        public static Report Create(string field, ReportCodeType reportCodeType) => Create((int)reportCodeType, field, ReportCodeMessage.GetMessage(reportCodeType));
    }
}
