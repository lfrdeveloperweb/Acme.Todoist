using System;
using System.Globalization;
using Acme.Todoist.Application.Core.Commons;

namespace Acme.Todoist.Infrastructure.Services
{
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
