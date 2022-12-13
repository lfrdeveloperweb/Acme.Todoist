using System;
using System.Globalization;

namespace Acme.Todoist.Infrastructure.Utils
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

    /// <summary>
    /// Represents an instant in time, typically expressed as a date and time of day.
    /// </summary>
    public class DateDateTimeProvider : IDateTimeProvider
    {
        private static readonly Lazy<CultureInfo> BrazilCultureInfoCache = new(() => CultureInfo.GetCultureInfo("pt-br"));

        private static readonly Lazy<TimeZoneInfo> BrazilianTimeZoneInfoCache = new(() =>
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Brazil/East");
            }
            catch (Exception)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }
        });

        /// <inheritdoc />
        public CultureInfo BrazilCultureInfo => BrazilCultureInfoCache.Value;

        /// <inheritdoc />
        public TimeZoneInfo BrazilianTimeZone => BrazilianTimeZoneInfoCache.Value;

        /// <inheritdoc />
        public DateTimeOffset BrasiliaNow => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, BrazilianTimeZoneInfoCache.Value);

        /// <inheritdoc />>
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public DateTime ConvertDateToBrasiliaDate(DateTime dateTime) => TimeZoneInfo.ConvertTime(dateTime, BrazilianTimeZoneInfoCache.Value);
    }
}
