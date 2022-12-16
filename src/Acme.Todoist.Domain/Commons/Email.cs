using System.Text.RegularExpressions;

namespace Acme.Todoist.Domain.Commons
{
    public sealed record Email
    {
        /// <summary>
        /// The email maximum length.
        /// </summary>
        public const int MaxLength = 128;

        /// <summary>
        /// Checks if the current string has a valid email address format.
        /// </summary>
        /// <param name="source">String to be checked.</param>
        /// <returns>Return true if the string has a valid email address format.</returns>
        public static bool IsValidEmailAddress(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase | RegexOptions.Compiled).IsMatch(source);
        }
    }
}
