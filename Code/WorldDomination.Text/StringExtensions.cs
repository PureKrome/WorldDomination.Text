using System;
using System.Text.RegularExpressions;

namespace WorldDomination.Text
{
    public static class StringExtensions
    {
        // REFERENCE: http://msdn.microsoft.com/en-us/library/844skk0h.aspx
        public static string Clean(this string content, string regexPattern = null)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }

            // Replace invalid characters with empty strings. 
            var pattern = string.IsNullOrEmpty(regexPattern) ? @"[^\w\ .@-]" : regexPattern;

            return Regex.Replace(content, pattern, string.Empty, RegexOptions.None);
        }
    }
}