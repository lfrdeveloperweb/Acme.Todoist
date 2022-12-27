using System.Text.RegularExpressions;

namespace Acme.Todoist.Domain.Commons
{
    public sealed record PhoneNumber
    {
        /// <summary>
        /// The phone number maximum length.
        /// </summary>
        public const int MaxLength = 16;

        /// <summary>
        /// Checks if the current string has a valid brazilian phone number format.
        /// </summary>
        /// <param name="source">String to be checked.</param>
        /// <returns>Return true if the string has a valid brazilian phone number format.</returns>
        public static bool IsValid(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return new Regex(@"\d{2,}\d{4,}\d{4}", RegexOptions.IgnoreCase | RegexOptions.Compiled).IsMatch(source);
        }
    }
}
