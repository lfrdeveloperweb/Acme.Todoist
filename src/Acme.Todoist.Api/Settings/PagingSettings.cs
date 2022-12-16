namespace Acme.Todoist.Api.Settings
{
    /// <summary>
    /// General configuration of the api
    /// </summary>
    public sealed record PagingSettings
    {
        /// <summary>
        /// Default value of records per page
        /// </summary>
        public int DefaultRecordsPerPage { get; init; }

        /// <summary>
        /// Limit of records per page
        /// </summary>
        public int MaxRecordsPerPage { get; init; }
    }
}
