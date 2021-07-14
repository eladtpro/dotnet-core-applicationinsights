using System.Text.RegularExpressions;

namespace ApplicationInsight.Web.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// match "*" any characters (zero or more) and "?" any character (one and only one)
        /// </summary>
        /// <param name="value">value to match</param>
        /// <param name="pattern">pattern to use with "*" and\or "?"</param>
        /// <returns></returns>
        public static bool IsMatch(this string value, string pattern)
        {
            bool match = Regex.IsMatch(value, WildCardToRegular(pattern));
            return match;
        }

        // If you want to implement both "*" and "?"
        private static string WildCardToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }
    }
}
