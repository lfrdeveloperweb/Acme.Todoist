using System;
using System.Globalization;

namespace Acme.Todoist.Application.Core.Commons
{
    /// <summary>
    /// Represents an instant in time, typically expressed as a date and time of day.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Brazil culture info.
        /// </summary>
        CultureInfo BrazilCultureInfo { get; }

        /// <summary>
        /// Brazil time zone.
        /// </summary>
        TimeZoneInfo BrazilianTimeZone { get; }

        /// <summary>
        /// An object whose value is the current UTC date and time.
        /// </summary>
        DateTimeOffset UtcNow { get; }

        /// <summary>
        /// An object whose value is the Brasilia date and time.
        /// </summary>
        DateTimeOffset BrasiliaNow { get; }

        /// <summary>
        /// An object whose value is the Brasilia date and time.
        /// </summary>
        DateTime ConvertDateToBrasiliaDate(DateTime dateTime);
    }
}
