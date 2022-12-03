using System.Text.Json.Serialization;

namespace Acme.Todoist.Commons.Models
{
    /// <summary>
    /// Configuration of the pagination.
    /// </summary>
    /// <param name="PageNumber">Page number.</param>
    /// <param name="RecordsPerPage">Number of records per page in response.</param>
    public sealed record PagingParameters(int PageNumber = 1, int RecordsPerPage = 20)
    {
        /// <summary>
        /// Offset from beginning of the filtered records.
        /// </summary>
        [JsonIgnore]
        public int Offset => RecordsPerPage * (PageNumber - 1);
    }
}
