using Company.App.Application.UseCases.DataExtraction.Models;
using System;
using static Company.App.Application.UseCases.DataMapping.Services.RegexSearchService;

namespace Company.App.Application.UseCases.DataMapping.Services
{
    public static class ExtractedDocumentSearch
    {
        /// <summary>
        /// Filters and returns lines from pages based on whether their text content matches any of the specified target
        /// strings.
        /// </summary>
        /// <remarks>If no valid targets are provided, no pages will be considered matching. The method
        /// does not modify the input collections.</remarks>
        /// <param name="lines">The collection of lines to filter. Each line should contain text and a page number.</param>
        /// <param name="targets">The set of target strings to search for within each line's text. Null, empty, or whitespace-only targets are
        /// ignored.</param>
        /// <param name="includeMatchingPages">If set to <see langword="true"/>, returns lines from pages containing at least one matching line; otherwise,
        /// returns lines from pages without any matches. The default is <see langword="true"/>.</param>
        /// <returns>An ordered collection of lines from pages that either contain or do not contain any of the target strings,
        /// depending on the value of <paramref name="includeMatchingPages"/>. The result is ordered by page number
        /// ascending and Y coordinate descending.</returns>
        public static IEnumerable<ExtractedLineDto> GetPagesByLineContent(IEnumerable<ExtractedLineDto> lines, IEnumerable<string> targets, bool includeMatchingPages = true)
        {
            var validTargets = targets
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();

            var targetPages = lines
                .Where(l => validTargets.Any(t => l.Text.Contains(t)))
                .Select(l => l.PageNumber)
                .Distinct()
                .ToHashSet();

            return lines
                .Where(l => includeMatchingPages
                    ? targetPages.Contains(l.PageNumber)
                    : !targetPages.Contains(l.PageNumber))
                .OrderBy(l => l.PageNumber)
                .ThenByDescending(l => l.Y)
                .ToList();
        }

        /// <summary>
        /// Retrieves the collection of lines that appear on the specified page, ordered from top to bottom and then
        /// left to right.
        /// </summary>
        /// <param name="lines">The collection of extracted lines to filter and order. Cannot be null.</param>
        /// <param name="pageNumber">The page number for which to retrieve lines.</param>
        /// <returns>An enumerable collection of lines on the specified page, ordered by descending Y coordinate and then
        /// ascending X coordinate. Returns an empty collection if no lines are found for the given page.</returns>
        public static IEnumerable<ExtractedLineDto> GetLinesOnPage(IEnumerable<ExtractedLineDto> lines, int pageNumber)
        {
            return lines
                .Where(l => l.PageNumber == pageNumber)
                .OrderByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .ToList();
        }

        /// <summary>
        /// Returns a collection of extracted lines that appear on pages within the specified range, ordered by page
        /// number and position.
        /// </summary>
        /// <remarks>If no lines are found within the specified page range, the returned collection will
        /// be empty.</remarks>
        /// <param name="lines">The collection of extracted lines to filter and order.</param>
        /// <param name="fromPage">The first page number in the inclusive range of pages to include. Must be less than or equal to <paramref
        /// name="toPage"/>.</param>
        /// <param name="toPage">The last page number in the inclusive range of pages to include. Must be greater than or equal to <paramref
        /// name="fromPage"/>.</param>
        /// <returns>An ordered collection of <see cref="ExtractedLineDto"/> objects that are located on pages from <paramref
        /// name="fromPage"/> to <paramref name="toPage"/>, inclusive. The collection is ordered first by page number,
        /// then by vertical position (Y), and then by horizontal position (X).</returns>
        public static IEnumerable<ExtractedLineDto> GetLinesOnPages(IEnumerable<ExtractedLineDto> lines, int fromPage, int toPage)
        {
            return lines
                .Where(l => l.PageNumber >= fromPage && l.PageNumber <= toPage)
                .OrderBy(l => l.PageNumber)
                .ThenByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .ToList();
        }

        /// <summary>
        /// Retrieves lines from a collection of PDF lines that follow the first occurrence of a line containing the
        /// specified target text, optionally including the target line itself.
        /// </summary>
        /// <remarks>Lines are grouped and processed by page, ordered by descending Y coordinate and
        /// ascending X coordinate within each page. Only the first occurrence of the target text per page is
        /// considered.</remarks>
        /// <param name="lines">The collection of PDF lines to search. Lines are grouped and processed by page number.</param>
        /// <param name="target">The text to search for within each line's content. The first line containing this text is used as the
        /// target.</param>
        /// <param name="followingLines">The number of lines to return after the target line. Must be zero or greater. If zero, all lines after the
        /// target (or including the target, if specified) are returned.</param>
        /// <param name="includeTargetLine">true to include the target line in the results; otherwise, false.</param>
        /// <returns>An enumerable collection of PDF lines following the target line, optionally including the target line
        /// itself. Returns an empty collection if the target text is not found or if the input collection is null.</returns>
        public static IEnumerable<ExtractedLineDto> GetLinesFromTargetLine(IEnumerable<ExtractedLineDto> lines, string target, int followingLines = 0, bool includeTargetLine = false)
        {
            if (lines == null)
                return Enumerable.Empty<ExtractedLineDto>();

            var result = lines
                .GroupBy(l => l.PageNumber)
                .OrderBy(g => g.Key)
                .SelectMany(pageGroup =>
                {
                    var pageLines = pageGroup
                        .OrderByDescending(l => l.Y)
                        .ThenBy(l => l.X)
                        .ToList();

                    var startIndex = pageLines.FindIndex(l => l.Text.Contains(target));

                    if (startIndex == -1)
                        return Enumerable.Empty<ExtractedLineDto>();

                    int skip = includeTargetLine ? startIndex : startIndex + 1;

                    var query = pageLines.Skip(skip);

                    if (followingLines > 0)
                    {
                        int take = includeTargetLine ? followingLines + 1 : followingLines;
                        query = query.Take(take);
                    }

                    return query;
                });

            return result.ToList();
        }

        /// <summary>
        /// Returns the collection of words from the specified line that are within a given vertical tolerance.
        /// </summary>
        /// <remarks>This method is useful for grouping words into lines when processing PDF text
        /// extraction, where minor Y coordinate variations may occur due to rendering or extraction
        /// differences.</remarks>
        /// <param name="words">The collection of words to filter. Each word must have page and position information.</param>
        /// <param name="line">The line model used to determine which words belong to the line. Must not be null.</param>
        /// <param name="yTolerance">The maximum allowed difference in the Y coordinate, in points, between a word and the line for the word to
        /// be considered part of the line. Defaults to 2.0.</param>
        /// <returns>An ordered collection of words that are on the same page and within the specified vertical tolerance of the
        /// line, sorted by their X coordinate. The collection is empty if no words match.</returns>
        public static IEnumerable<ExtractedWordDto> GetWordsFromLine(IEnumerable<ExtractedWordDto> words, ExtractedLineDto line, double yTolerance = 2.0)
        {
            return words
                .Where(w =>
                    w.PageNumber == line.PageNumber &&
                    Math.Abs(w.Y - line.Y) <= yTolerance)
                .OrderBy(w => w.X)
                .ToList();
        }

        /// <summary>
        /// Searches for the first line in the collection whose text contains the specified substring, using a
        /// case-insensitive comparison.
        /// </summary>
        /// <param name="lines">The collection of lines to search.</param>
        /// <param name="text">The substring to search for within each line's text. The search is case-insensitive. If null, empty, or
        /// whitespace, the method returns null.</param>
        /// <returns>An instance of ExtractedLineDto representing the first matching line if found; otherwise, null.</returns>
        public static ExtractedLineDto GetFirstLineContaining(IEnumerable<ExtractedLineDto> lines, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return lines.FirstOrDefault(l =>
            l.Text.Contains(text, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Searches for the first line on a specified page that contains the given text, ordered from top to bottom and
        /// then left to right.
        /// </summary>
        /// <remarks>Lines are ordered by descending Y coordinate (top to bottom) and then by ascending X
        /// coordinate (left to right) before searching for the specified text.</remarks>
        /// <param name="line">The collection of extracted lines to search within. Cannot be null.</param>
        /// <param name="text">The text to search for within each line. The search is case-insensitive.</param>
        /// <param name="pageNumber">The page number to restrict the search to.</param>
        /// <returns>An instance of ExtractedLineDto representing the first matching line if found; otherwise, null.</returns>
        public static ExtractedLineDto GetFirstLineContainingByPage(IEnumerable<ExtractedLineDto> lines, string text, int pageNumber)
        {
            var line = GetLinesOnPage(lines, pageNumber);

            return line
                .Where(l => l.PageNumber == pageNumber)
                .OrderByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .FirstOrDefault(l =>
                l.Text.Contains(text, StringComparison.OrdinalIgnoreCase));
        }
                
        /// <summary>
        /// Finds the first word in the collection that matches the specified width, ordered by descending Y coordinate
        /// and then ascending X coordinate.
        /// </summary>
        /// <param name="words">The collection of words to search. Cannot be null.</param>
        /// <param name="width">The width, in units, to match against the words' widths.</param>
        /// <returns>An instance of ExtractedWordDto that matches the specified width and ordering criteria, or null if no such
        /// word is found.</returns>
        public static ExtractedWordDto GetWordByWidth(IEnumerable<ExtractedWordDto> words, double width)
        {
            return words
                .Where(w => w.Width == width)
                .OrderByDescending(w => w.Y)
                .ThenBy(w => w.X)
                .FirstOrDefault();
        }

        /// <summary>
        /// Finds the first word from the collection that has the specified number of characters, ordering by vertical
        /// and then horizontal position.
        /// </summary>
        /// <remarks>If multiple words have the same number of characters, the word with the highest Y
        /// value (lowest on the page) and then the lowest X value (leftmost) is returned.</remarks>
        /// <param name="words">The collection of words to search. Each word is represented by an ExtractedWordDto instance.</param>
        /// <param name="charInString">The exact number of characters the returned word must contain. Must be non-negative.</param>
        /// <returns>An instance of ExtractedWordDto that matches the specified width and ordering criteria, or null if no such
        /// word is found.</returns>
        public static ExtractedWordDto GetWordByNumberOfChar(IEnumerable<ExtractedWordDto> words, int charInString)
        {
            return words
                .Where(w => w.Text.Length == charInString)
                .OrderByDescending(w => w.Y)
                .ThenBy(w => w.X)
                .FirstOrDefault();

        }

        /// <summary>
        /// Extracts the names of companies listed in the specified line of text.
        /// </summary>
        /// <param name="line">The line of text from which to extract company names. Cannot be null.</param>
        /// <returns>An enumerable collection of company names found in the line. The collection is empty if no company names are
        /// found.</returns>
        public static IEnumerable<string> GetCompaniesListedFromLine(ExtractedLineDto line)
        {
            var companies = SplitCompanies(line.Text);

            return companies;
        }

        /// <summary>
        /// Retrieves the text that appears immediately after the specified label in the first line containing that
        /// label.
        /// </summary>
        /// <remarks>If multiple lines contain the label, only the first occurrence is considered. Leading
        /// spaces, colons, and hyphens are trimmed from the result.</remarks>
        /// <param name="lines">A collection of lines to search for the specified label. Each line is represented by an ExtractedLineDto
        /// object.</param>
        /// <param name="label">The label to search for within the lines. The search is case-insensitive.</param>
        /// <returns>A string containing the text found after the specified label in the first matching line. Returns an empty
        /// string if the label is not found.</returns>
        public static string GetValueAfterLabel(IEnumerable<ExtractedLineDto> lines, string label)
        {
            var line = lines.FirstOrDefault(l =>
            l.Text.Contains(label, StringComparison.OrdinalIgnoreCase));
            if (line is null)
                return string.Empty;

            var index = line.Text.IndexOf(label, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
                return string.Empty;

            return line.Text[(index + label.Length)..].Trim(' ', ':', '-');
        }

        /// <summary>
        /// Returns the concatenated text of words within the specified horizontal range in the given line.
        /// </summary>
        /// <param name="wordsList">The collection of extracted words to search within.</param>
        /// <param name="line">The line from which to extract words based on their horizontal position.</param>
        /// <param name="start">The inclusive minimum X-coordinate value. Only words with an X position greater than or equal to this value
        /// are included.</param>
        /// <param name="end">The inclusive maximum X-coordinate value. Only words with an X position less than or equal to this value are
        /// included.</param>
        /// <returns>A string containing the text of all words in the specified line whose X-coordinate falls within the range
        /// from start to end, separated by spaces. Returns an empty string if no words match the criteria.</returns>
        public static string GetPartOfLineRelativeToX(IEnumerable<ExtractedWordDto> wordsList, ExtractedLineDto line, int start, int end)
        {
            var words = GetWordsFromLine(wordsList, line);

            return string.Join(" ", GetWordsFromLine(wordsList, line)
                .Where(w => w.X >= start && w.X <= end)
                .Select(w => w.Text));
        }
        /// <summary>
        /// Retrieves the nth word from the specified input string.
        /// </summary>
        /// <remarks>Words are identified by splitting the input on spaces, tabs, and newline characters.
        /// Consecutive delimiters are treated as a single separator.</remarks>
        /// <param name="input">The string to search for words. Words are delimited by spaces, tabs, or newline characters.</param>
        /// <param name="n">The one-based index of the word to retrieve. Must be greater than 0 and less than or equal to the number of
        /// words in the input.</param>
        /// <returns>The nth word in the input string if it exists; otherwise, an empty string.</returns>
        public static string GetNthWordInString(string input, int n)
        {
            var words = input.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
                return string.Empty;

            if (n > 0 && n <= words.Length)
                return words[n - 1];

            if (n < 0 && Math.Abs(n) <= words.Length)
                return words[words.Length + n]; // e.g. -1 => last

            return string.Empty;
        }

        /// <summary>
        /// Determines the corresponding regular expression search service type for the specified pattern value.
        /// </summary>
        /// <param name="pattern">An integer representing the pattern value to evaluate. Must correspond to a defined value in the
        /// RegexSearchServiceType enumeration.</param>
        /// <returns>A value of the RegexSearchServiceType enumeration that matches the specified pattern value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if pattern does not correspond to a defined value in the RegexSearchServiceType enumeration.</exception>
        internal static RegexSearchServiceType GetPatternType(int pattern)
        {
            if (!Enum.IsDefined(typeof(RegexSearchServiceType), pattern))
                throw new ArgumentOutOfRangeException(nameof(pattern), $"Invalid pattern value: {pattern}");

            var patternType = (RegexSearchServiceType)pattern;

            return patternType;
        }
        /// <summary>
        /// Extracts a value from the specified line of text using the given pattern type and optional constraints.
        /// </summary>
        /// <remarks>The method supports multiple extraction patterns, such as numeric values of a
        /// specific length, price formats, email addresses, dates, and substrings between delimiters. Ensure that all
        /// required parameters for the chosen patternType are supplied to avoid exceptions.</remarks>
        /// <param name="patternType">The type of pattern to use for extracting a value from the line. Determines the extraction logic applied.</param>
        /// <param name="line">The input string from which to extract the value.</param>
        /// <param name="minLength">The minimum length of the numeric value to extract. Required when patternType is
        /// GetNumericValueInStringByLength.</param>
        /// <param name="maxLength">The maximum length of the numeric value to extract. Required when patternType is
        /// GetNumericValueInStringByLength.</param>
        /// <param name="start">The starting delimiter for extracting a substring. Required when patternType is
        /// GetStringValueInLineBetweenValues.</param>
        /// <param name="end">The ending delimiter for extracting a substring. Required when patternType is
        /// GetStringValueInLineBetweenValues.</param>
        /// <returns>A string containing the extracted value based on the specified pattern and constraints, or an empty string
        /// if no match is found.</returns>
        /// <exception cref="ArgumentException">Thrown when required parameters for the selected patternType are not provided.</exception>
        /// <exception cref="NotSupportedException">Thrown when the specified patternType is not supported.</exception>
        public static string GetValueByPattern(RegexSearchServiceType patternType, string line, int? minLength = null, int? maxLength = null, string? start = null, string? end = null)
        {
            return patternType switch
            {
                RegexSearchServiceType.GetNumericValueInStringByLength
                    when minLength.HasValue && maxLength.HasValue
                        => GetNumericValueInStringByLength(line, minLength.Value, maxLength.Value),

                RegexSearchServiceType.GetPriceFormatValueInString
                        => GetPriceFormatValueInString(line),

                RegexSearchServiceType.GetEmailValueInString
                        => GetEmailValueInString(line),

                RegexSearchServiceType.GetDateValueByDDdotMMdotYYYYByPage
                        => GetDateValueByDDdotMMdotYYYYByPage(line),

                RegexSearchServiceType.GetStringValueInLineBetweenValuesContainingColon
                    when !string.IsNullOrWhiteSpace(start) && !string.IsNullOrWhiteSpace(end)
                        => GetStringValueInLineBetweenValuesContainingColon(line, start, end),

                RegexSearchServiceType.GetNumericValueInStringByLength
                        => throw new ArgumentException("minLength and maxLength are required."),

                RegexSearchServiceType.GetStringValueInLineBetweenValuesContainingColon
                        => throw new ArgumentException("start and end are required."),

                _ => throw new NotSupportedException($"Pattern type '{patternType}' is not supported.")
            };
        }

        /// <summary>
        /// Extracts a substring from the specified line that matches the given pattern and optional constraints.
        /// </summary>
        /// <param name="pattern">An integer representing the pattern type to use for extraction. The value determines which pattern is
        /// applied.</param>
        /// <param name="line">The input string from which to extract the value based on the specified pattern.</param>
        /// <param name="minLength">The optional minimum length of the substring to extract. If specified, only substrings of at least this
        /// length are considered.</param>
        /// <param name="maxLength">The optional maximum length of the substring to extract. If specified, only substrings up to this length are
        /// considered.</param>
        /// <param name="start">An optional string that specifies the required starting sequence of the substring to extract. If provided,
        /// the extracted value must begin with this string.</param>
        /// <param name="end">An optional string that specifies the required ending sequence of the substring to extract. If provided, the
        /// extracted value must end with this string.</param>
        /// <returns>A substring from the input line that matches the specified pattern and constraints, or null if no match is
        /// found.</returns>
        public static string GetValueByPattern(int pattern, string line, int? minLength = null, int? maxLength = null, string? start = null, string? end = null)
        {
            var patternType = GetPatternType(pattern);

            return GetValueByPattern(patternType, line, minLength, maxLength, start, end);
        }

        /// <summary>
        /// Extracts a value from the first line in the collection that contains the specified content, using the given
        /// pattern and optional constraints.
        /// </summary>
        /// <param name="lines">The collection of lines to search for the target content.</param>
        /// <param name="targetLineContent">The text to locate within the lines. The method searches for the first line containing this value.</param>
        /// <param name="pattern">The pattern identifier used to extract the value from the matched line.</param>
        /// <param name="minLength">The minimum length, in characters, of the value to extract. If null, no minimum length is enforced.</param>
        /// <param name="maxLength">The maximum length, in characters, of the value to extract. If null, no maximum length is enforced.</param>
        /// <param name="start">An optional string that the extracted value must start with. If null, no start constraint is applied.</param>
        /// <param name="end">An optional string that the extracted value must end with. If null, no end constraint is applied.</param>
        /// <returns>A string containing the extracted value if a matching line and pattern are found; otherwise, an empty
        /// string.</returns>
        public static string GetValueByLineAndPattern(IEnumerable<ExtractedLineDto> lines, string targetLineContent, int pattern, int? minLength = null, int? maxLength = null, string? start = null, string? end = null)
        {
            var line = GetFirstLineContaining(lines, targetLineContent);

            if (line == null)
                return string.Empty;

            var patternType = GetPatternType(pattern);

            return GetValueByPattern(patternType, line.Text, minLength, maxLength, start, end);
        }

        /// <summary>
        /// Removes specific characters from the specified string, optionally ignoring certain characters.
        /// </summary>
        /// <param name="text">The input string from which characters will be removed. Cannot be null.</param>
        /// <param name="ignore">A string containing characters to ignore during removal. If empty, all target characters are removed.</param>
        /// <returns>A new string with the specified characters removed, except those specified to be ignored.</returns>
        public static string RemoveCharFromString(string text, string ignore = "")
        {
            if (ignore == "")
                return TrimCharFromString(text);
            return TrimCharFromString(text, ignore);
        }

        /// <summary>
        /// Removes all numeric characters from the specified string, with the option to retain certain digits.
        /// </summary>
        /// <param name="text">The input string from which numeric characters will be removed.</param>
        /// <param name="include">A string containing digits to retain in the result. Digits specified in this parameter will not be removed
        /// from the input string. If empty, all digits are removed.</param>
        /// <returns>A new string with numeric characters removed, except for any digits specified in the include parameter.</returns>
        public static string RemoveNumbersFromString(string text, string include = "")
        {
            if (include == "")
                return TrimNumbersFromString(text);
            return TrimNumbersFromString(text, include);
        }

        /// <summary>
        /// Gets the highest page number found in the specified collection of extracted document lines.
        /// </summary>
        /// <param name="document">A collection of extracted lines representing the contents of a document. Each line should include a page
        /// number.</param>
        /// <returns>The highest page number present in the collection. Returns 0 if the collection is empty.</returns>
        public static int GetNumbersofPagesInFile(IEnumerable<ExtractedLineDto> document)
        {
            return document.Select(d => d.PageNumber)
                .OrderByDescending(d => d)
                .FirstOrDefault();
        }
    }
}
