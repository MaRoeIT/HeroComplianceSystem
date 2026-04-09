using Company.App.Application.UseCases.DataMapping.Models;
using System.Text.RegularExpressions;


namespace Company.App.Application.UseCases.DataMapping.Services
{
    public static class RegexSearchService
    {
        public enum RegexSearchServiceType
        {
            GetNumericValueInStringByLength = 1,
            GetPriceFormatValueInString = 2,
            GetEmailValueInString = 3,
            GetDateValueByDDdotMMdotYYYYByPage = 4,
            GetStringValueInLineBetweenValuesContainingColon = 5,

        }
        /// <summary>
        /// Searches the specified text for the first sequence of consecutive numeric digits whose length is within the
        /// specified range.
        /// </summary>
        /// <param name="text">The input string to search for numeric sequences.</param>
        /// <param name="minLength">The minimum number of consecutive digits to match. Must be greater than zero and less than or equal to
        /// <paramref name="maxLength"/>.</param>
        /// <param name="maxLength">The maximum number of consecutive digits to match. Must be greater than or equal to <paramref
        /// name="minLength"/>.</param>
        /// <returns>A string containing the first sequence of digits found within the specified length range; or an empty string
        /// if no such sequence exists.</returns>
        public static string GetNumericValueInStringByLength(string text, int minLength, int maxLength)
        {
            string pattern = $@"\d{{{minLength},{maxLength}}}";

            var match = Regex.Match(text, pattern);

            return match.Success ? match.Value : string.Empty;
        }

        /// <summary>
        /// Extracts the first price value in standard currency format from the specified text line using a regular
        /// expression.
        /// </summary>
        /// <remarks>The method searches for price values matching the pattern "1,234.56" or "123.45".
        /// Only the first occurrence is returned. The method does not validate currency symbols or locale-specific
        /// formats.</remarks>
        /// <param name="line">The input string to search for a price value. Can be any line of text.</param>
        /// <returns>A string containing the first price value found in the input line, formatted as a number with optional
        /// thousands separators and two decimal places. Returns an empty string if no price value is found.</returns>
        public static string GetPriceFormatValueInString(string line)
        {
            string pattern = @"\d{1,3}(,\d{3})*\.\d{2}";

            var match = Regex.Match(line, pattern);

            return match.Success ? match.Value : string.Empty;

        }

        /// <summary>
        /// Extracts the first email address found in the specified string.
        /// </summary>
        /// <remarks>The search is performed using a regular expression that matches standard email
        /// address formats surrounded by whitespace. Only the first match is returned.</remarks>
        /// <param name="line">The input string to search for an email address. Can be null or empty.</param>
        /// <returns>A string containing the first email address found in the input, or an empty string if no email address is
        /// present.</returns>
        public static string GetEmailValueInString(string line)
        {
            string pattern = "[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}";

            var match = Regex.Match(line, pattern);

            return match.Success ? match.Value : string.Empty;
        }

        /// <summary>
        /// Extracts the first date in the format "dd.MM.yyyy" from the specified input string.
        /// </summary>
        /// <param name="line">The input string to search for a date in the format "dd.MM.yyyy".</param>
        /// <returns>A string containing the first date found in the format "dd.MM.yyyy"; or an empty string if no such date is
        /// present.</returns>
        public static string GetDateValueByDDdotMMdotYYYYByPage(string line)
        {
            string pattern = @"\d{2}\.\d{2}\.\d{4}";

            var match = Regex.Match(line, pattern);

            return match.Success ? match.Value : String.Empty;
        }

        /// <summary>
        /// Extracts the substring located between the specified start and end markers within a single line of text.
        /// </summary>
        /// <remarks>The method performs a case-sensitive search and expects the substring to be composed
        /// of letters (including Scandinavian characters), hyphens, and spaces. Whitespace around the markers and the
        /// value is ignored.</remarks>
        /// <param name="line">The input string to search for the substring. Must not be null.</param>
        /// <param name="start">The marker that precedes the desired substring. Cannot be null or empty.</param>
        /// <param name="end">The marker that follows the desired substring. Cannot be null or empty.</param>
        /// <returns>A string containing the value found between the start and end markers. Returns an empty string if no match
        /// is found.</returns>
        public static string GetStringValueInLineBetweenValuesContainingColon(string line, string start, string? end = null)
        {
            string startPattern = string.IsNullOrEmpty(start) || start == "^"
                ? "^"
                : Regex.Escape(start);

            string endPattern = string.IsNullOrEmpty(end) || end == "$"
                ? "$"
                : Regex.Escape(end);

            string pattern = $@"{startPattern}\s*:\s*(.+?)\s*{endPattern}";

            var match = Regex.Match(line, pattern);

            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        /// <summary>
        /// Removes all numeric characters from the specified string and trims any leading or trailing whitespace.
        /// </summary>
        /// <param name="text">The input string from which numeric characters will be removed. Can be null or empty.</param>
        /// <returns>A new string with all numeric characters removed and whitespace trimmed. Returns an empty string if the
        /// input is null or consists only of numbers and whitespace.</returns>
        public static string TrimNumbersFromString(string text)
        {
            text = Regex.Replace(text, @"\d+", "").Trim();

            return text;
        }

        /// <summary>
        /// Removes numeric characters from the specified string, except when the numbers are immediately preceded by
        /// any character in the provided set.
        /// </summary>
        /// <remarks>If the include parameter is null or empty, all numeric characters are removed from
        /// the input string. The comparison is case-sensitive.</remarks>
        /// <param name="text">The input string from which numeric characters will be removed.</param>
        /// <param name="include">A string containing characters that, when immediately preceding a number, prevent that number from being
        /// removed.</param>
        /// <returns>A new string with numeric characters removed, except for those numbers that are immediately preceded by any
        /// character in the include set.</returns>
        public static string TrimNumbersFromString(string text, string include)
        {
            if (string.IsNullOrEmpty(include))
                return TrimNumbersFromString(text);

            var escaped = Regex.Escape(include);

            // Remove numbers NOT preceded by allowed symbols
            var pattern = $@"(?<![{escaped}])\d+";

            return Regex.Replace(text, pattern, "").Trim();
        }

        /// <summary>
        /// Removes all non-digit characters from the specified string and trims any leading or trailing whitespace.
        /// </summary>
        /// <param name="text">The input string from which to remove non-digit characters. Can be null or empty.</param>
        /// <returns>A string containing only the digit characters from the input, with leading and trailing whitespace removed.
        /// Returns an empty string if no digits are present.</returns>
        public static string TrimCharFromString(string text)
        {
            text = Regex.Replace(text, @"[^\d]+", "").Trim();
            
            return text;
        }

        /// <summary>
        /// Removes all characters from the specified string except digits and the characters provided in the ignore
        /// list.
        /// </summary>
        /// <remarks>This method uses regular expressions to filter characters. The ignore list is treated
        /// literally; special characters are escaped automatically.</remarks>
        /// <param name="text">The input string to process. May be null or empty.</param>
        /// <param name="ignore">A string containing characters to preserve in addition to digits. If null or empty, only digits are
        /// preserved.</param>
        /// <returns>A new string containing only digits and the specified ignored characters from the input. Returns an empty
        /// string if no such characters are present.</returns>
        public static string TrimCharFromString(string text, string ignore)
        {
            if (string.IsNullOrEmpty(ignore))
                return TrimCharFromString(text);

            // Escape special regex characters in ignore string
            var escapedIgnore = Regex.Escape(ignore);

            // Build pattern: keep digits + ignored characters
            var pattern = $@"[^\d{escapedIgnore}]+";

            return Regex.Replace(text, pattern, "").Trim();
        }

        /// <summary>
        /// Splits a string containing multiple company names into individual company names based on common company
        /// suffixes.
        /// </summary>
        /// <remarks>Company names are identified by matching common suffixes such as AS, LTDA, INC, LTD,
        /// GMBH, and SA. The method trims whitespace from each extracted company name.</remarks>
        /// <param name="line">The input string containing one or more company names to be split. Cannot be null.</param>
        /// <returns>An enumerable collection of company names extracted from the input string. The collection will be empty if
        /// no company names are found.</returns>
        public static IEnumerable<string> SplitCompanies(string line)
        {
            var pattern = @"(.+?\b(?:AB|AS|LTDA|INC|LTD|GMBH|SA)\b)";

            var matches = Regex.Matches(line, pattern);

            return matches
                .Cast<Match>()
                .Select(m => m.Value.Trim())
                .ToList();

        }

        /// <summary>
        /// Attempts to parse a section header from the specified text line and returns a corresponding SectionDto if
        /// successful.
        /// </summary>
        /// <remarks>The method expects the input to start with a section number (e.g., "1.", "2.1.")
        /// followed by a title. Trailing colons in the title are removed. Returns null if the input is null, empty, or
        /// does not match the expected pattern.</remarks>
        /// <param name="text">The text line to parse. Expected to contain a section number followed by a title (e.g., "1.2.3. Section
        /// Title").</param>
        /// <returns>A SectionDto representing the parsed section if the text matches the expected format; otherwise, null.</returns>
        public static SectionDto? TryParseSectionFromLine(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            string pattern = @"^(?<number>\d+(?:\.\d+)*)\.\s*(?<title>.+?)\s*$";

            var match = Regex.Match(text, pattern);

            if (!match.Success)
                return null;

            var sectionNumber = match.Groups["number"].Value;
            var title = match.Groups["title"].Value.Trim().TrimEnd(':');
            var level = sectionNumber.Count(c => c == '.') + 1;

            return new SectionDto(sectionNumber, title, level);
        }

        public static string RemoveLabel(string? text, string label)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return Regex.Replace(text, $@"{Regex.Escape(label)}\s*:?\s*", "").Trim();
        }

        /// <summary>
        /// Searches the specified text for the first substring that matches the given regular expression pattern.
        /// </summary>
        /// <param name="text">The input string to search for a matching substring.</param>
        /// <param name="pattern">The regular expression pattern to match within the input text.</param>
        /// <returns>The first substring in the input text that matches the specified pattern, or an empty string if no match is
        /// found.</returns>
        public static string ContextTrimParse(string text, string pattern)
        {
            var match = Regex.Match(text, pattern);

            return match.Success ? match.Value : string.Empty;
        }
    }
}
