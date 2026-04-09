using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Text.RegularExpressions;
using static Company.App.Application.UseCases.DataMapping.Services.RegexSearchService;
using static Company.App.Application.UseCases.DataMapping.Helper.IsBulletLine;
using static Company.App.Application.UseCases.DataMapping.Helper.IsItemLine;
using static Company.App.Application.UseCases.DataMapping.Helper.IsPriceFormatValues;

namespace Company.App.Application.UseCases.DataMapping.Services
{
    /// <summary>
    /// Provides static methods for searching, filtering, and extracting structured information from collections of
    /// extracted document lines and words, such as those obtained from PDF text extraction.
    /// </summary>
    /// <remarks>The methods in this class support a variety of document analysis scenarios, including
    /// locating lines or words by content, extracting values using patterns, grouping lines into sections or items, and
    /// handling document-specific structures such as purchase orders. All methods are stateless and do not modify the
    /// input collections. This class is intended for use in document processing pipelines where accurate extraction and
    /// organization of textual data is required.</remarks>
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
        /// Returns a collection of lines that follow each target line within the specified input, optionally including
        /// the target lines themselves.
        /// </summary>
        /// <remarks>Lines are grouped and processed by page number, and ordering is determined by Y
        /// (descending) and X (ascending) coordinates. Duplicate lines in the result are removed. Only target lines
        /// with non-empty text are considered for matching.</remarks>
        /// <param name="lines">The sequence of lines to search for target matches and extract following lines from. Cannot be null.</param>
        /// <param name="targets">The sequence of target lines whose text is used to identify matches within the input lines. Only targets
        /// with non-empty text are considered. Cannot be null.</param>
        /// <param name="followingLines">The number of lines to include after each matched target line. Must be zero or greater.</param>
        /// <param name="includeTargetLine">true to include the target line itself in the result; otherwise, false.</param>
        /// <returns>An enumerable collection of lines that follow each matched target line, optionally including the target
        /// lines. Returns an empty collection if no matches are found or if the input sequences are null.</returns>
        public static IEnumerable<ExtractedLineDto> GetLinesFromTargetLines(IEnumerable<ExtractedLineDto> lines, IEnumerable<ExtractedLineDto> targets, int followingLines = 0, bool includeTargetLine = false)
        {
            if (lines == null || targets == null)
                return Enumerable.Empty<ExtractedLineDto>();

            var targetList = targets
                .Where(t => !string.IsNullOrWhiteSpace(t.Text))
                .ToList();

            if (!targetList.Any())
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

                    var collected = new List<ExtractedLineDto>();

                    for (int i = 0; i < pageLines.Count; i++)
                    {
                        bool isMatch = targetList.Any(target => pageLines[i].Text.Contains(target.Text));

                        if (!isMatch)
                            continue;

                        int startIndex = includeTargetLine ? i : i + 1;

                        if (startIndex >= pageLines.Count)
                            continue;

                        IEnumerable<ExtractedLineDto> selection = pageLines.Skip(startIndex);

                        if (followingLines > 0)
                        {
                            int take = includeTargetLine ? followingLines + 1 : followingLines;
                            selection = selection.Take(take);
                        }

                        collected.AddRange(selection);
                    }

                    return collected;
                });

            return result.Distinct().ToList();
        }

        /// <summary>
        /// Returns a sequence of lines between the first occurrence of a start target string and the first subsequent
        /// occurrence of an end target string within the provided collection.
        /// </summary>
        /// <remarks>The method orders the input lines before searching for the target strings. Only the
        /// first occurrence of each target is considered. If the start target appears after the end target, or if
        /// either target is not found, the result is empty.</remarks>
        /// <param name="lines">The collection of lines to search. The lines are expected to contain text and positional information.</param>
        /// <param name="startTarget">The string to identify the starting line. The search begins at the first line whose text contains this
        /// value.</param>
        /// <param name="endTarget">The string to identify the ending line. The search ends at the first line after the start whose text
        /// contains this value.</param>
        /// <param name="includeTargets">true to include the lines containing the start and end target strings in the result; otherwise, false.</param>
        /// <returns>An enumerable collection of lines found between the start and end target lines, ordered by page number, Y,
        /// and X coordinates. Returns an empty collection if the start or end target is not found, or if the input
        /// collection is null.</returns>
        public static IEnumerable<ExtractedLineDto> GetLinesFromTargetLineToTargetLine(IEnumerable<ExtractedLineDto> lines, string startTarget, string endTarget, bool includeTargets)
        {
            if (lines == null)
                return Enumerable.Empty<ExtractedLineDto>();

            var result = lines
                .OrderBy(l => l.PageNumber)
                .ThenByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .ToList();

            var startIndex = result.FindIndex(l => l.Text.Contains(startTarget));
            if (startIndex == -1)
                return Enumerable.Empty<ExtractedLineDto>();

            var endIndex = result
                .Skip(startIndex + 1)
                .Select((line, index) => new { line, index })
                .FirstOrDefault(x => x.line.Text.Contains(endTarget));

            if (endIndex == null)
                return Enumerable.Empty<ExtractedLineDto>();

            int realEndIndex = startIndex + 1 + endIndex.index;

            int from = includeTargets ? startIndex : startIndex + 1;
            int to = includeTargets ? realEndIndex : realEndIndex - 1;

            if (from > to)
                return Enumerable.Empty<ExtractedLineDto>();

            return result
                .Skip(from)
                .Take(to - from + 1)
                .ToList();
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
        /// Returns a collection of words that are associated with the specified lines, matching by page number and
        /// vertical position within a given tolerance.
        /// </summary>
        /// <remarks>The returned words are ordered first by page number, then by descending vertical
        /// position, and finally by horizontal position. This method is useful for grouping words with their
        /// corresponding text lines in document processing scenarios.</remarks>
        /// <param name="words">The collection of words to filter and associate with lines. Cannot be null.</param>
        /// <param name="lines">The collection of lines to match words against. Cannot be null.</param>
        /// <param name="yTolerance">The maximum allowed difference in vertical position, in units, between a word and a line for them to be
        /// considered associated. Defaults to 2.0.</param>
        /// <returns>An ordered collection of words that are matched to the provided lines based on page number and vertical
        /// proximity. Returns an empty collection if either input is null.</returns>
        public static IEnumerable<ExtractedWordDto> GetWordsFromLines(IEnumerable<ExtractedWordDto> words, IEnumerable<ExtractedLineDto> lines, double yTolerance = 2.0)
        {
            if (words == null || lines == null)
                return Enumerable.Empty<ExtractedWordDto>();

            return lines
                .SelectMany(l => words
                    .Where(w =>
                        w.PageNumber == l.PageNumber &&
                        Math.Abs(w.Y - l.Y) <= yTolerance))
                .OrderBy(w => w.PageNumber)
                .ThenByDescending(w => w.Y)
                .ThenBy(w => w.X)
                .ToList();
        }

        /// <summary>
        /// Returns the nth word from each line, based on horizontal position, within a specified vertical tolerance.
        /// </summary>
        /// <remarks>Words are matched to lines based on page number, horizontal position within the line
        /// bounds, and vertical proximity as defined by yTolerance. Words in each line are ordered by their X
        /// coordinate before selecting the nth word.</remarks>
        /// <param name="words">A collection of words to search, each with positional and page information.</param>
        /// <param name="lines">A collection of lines that define the regions in which to search for words.</param>
        /// <param name="n">The one-based index of the word to extract from each line. Must be greater than or equal to 1.</param>
        /// <param name="yTolerance">The maximum allowed difference in the Y coordinate between a word and a line for the word to be considered
        /// part of the line. Defaults to 2.0.</param>
        /// <returns>An enumerable collection containing the nth word from each line where such a word exists. The collection is
        /// empty if no matching words are found.</returns>
        public static IEnumerable<ExtractedWordDto> GetNthWordInLines(IEnumerable<ExtractedWordDto> words, IEnumerable<ExtractedLineDto> lines, int n, double yTolerance = 2.0)
        {
            if (words == null || lines == null || n < 1)
                return Enumerable.Empty<ExtractedWordDto>();

            return lines
                .Select(line =>
                {
                    var lineWords = words
                        .Where(w =>
                            w.PageNumber == line.PageNumber &&
                            Math.Abs(w.Y - line.Y) <= yTolerance &&
                            w.X >= line.X &&
                            w.X <= line.X + line.Width)
                        .OrderBy(w => w.X)
                        .ToList();

                    return lineWords.Count >= n ? lineWords[n - 1] : null;
                })
                .Where(w => w != null)!;
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
        /// Searches for the first line containing the specified label and returns its value with the label removed.
        /// </summary>
        /// <param name="lines">A collection of extracted lines to search for the specified label.</param>
        /// <param name="label">The label to search for within the text of each line.</param>
        /// <returns>The value from the first line that contains the specified label, with the label removed. Returns null if no
        /// such line is found.</returns>
        public static string GetValueByLabel(IEnumerable<ExtractedLineDto> lines, string label)
        {
            var text = lines
                .Select(l => l.Text)
                .FirstOrDefault(t => t.Contains(label));

            return RemoveLabel(text, label);
        }

        /// <summary>
        /// Retrieves the text value that appears immediately after the specified label in the first matching line.
        /// </summary>
        /// <remarks>Leading spaces, colons, and hyphens are trimmed from the extracted value. Only the
        /// first occurrence of the label is considered.</remarks>
        /// <param name="lines">A collection of extracted lines to search for the label. Each line is represented by an ExtractedLineDto
        /// object.</param>
        /// <param name="label">The label to search for within the lines. The search is case-insensitive.</param>
        /// <returns>A string containing the text found after the specified label in the first matching line. Returns an empty
        /// string if the label is not found.</returns>
        public static string GetValueAfterLabel(IEnumerable<ExtractedLineDto> lines, string label)
        {
            var line = lines.FirstOrDefault(l =>
                !string.IsNullOrWhiteSpace(l.Text) &&
                l.Text.Contains(label, StringComparison.OrdinalIgnoreCase));

            if (line == null)
                return string.Empty;

            var index = line.Text.IndexOf(label, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
                return string.Empty;

            return line.Text[(index + label.Length)..]
                .Trim(' ')
                .Trim(':')
                .Trim('-');
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

        //****************************************************
        //************SPECIALISED SOLUTIONS BELOW*************
        //****************************************************

        //************PURCHASE ORDER**************************

        /// <summary>
        /// Filters and returns item lines from a collection of extracted lines based on specific criteria related to
        /// price formatting and word count.
        /// </summary>
        /// <remarks>A line is considered an item line if it contains price-formatted values, consists of
        /// exactly eight words, and does not contain the text "Your ref". This method is typically used to identify
        /// itemized entries in purchase order documents.</remarks>
        /// <param name="lines">The collection of extracted line data to evaluate for item line criteria.</param>
        /// <param name="words">The collection of extracted word data used to determine the word count for each line.</param>
        /// <returns>An enumerable collection of extracted line data that meet the item line criteria. The collection will be
        /// empty if no lines match the criteria.</returns>
        public static IEnumerable<ExtractedLineDto> GetItemLines(IEnumerable<ExtractedLineDto> lines, IEnumerable<ExtractedWordDto> words)
        {
            return lines
                .Where(l =>
                    HasPriceFormatValues(l.Text, 2) &&
                    GetWordsFromLine(words, l).Count() == 8 &&
                    !l.Text.Contains("Your ref"))
                .OrderBy(l => l.PageNumber)
                .ThenByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .ToList();
        }

        /// <summary>
        /// Extracts content blocks for each item line from a collection of extracted lines.
        /// </summary>
        /// <remarks>Lines are grouped and ordered by page number and position to determine content
        /// blocks. Only lines with non-empty text are considered. The method skips lines that should be ignored or are
        /// positioned below a certain threshold.</remarks>
        /// <param name="allLines">The complete collection of extracted lines to search for item content. Lines must not be null and should
        /// contain non-whitespace text.</param>
        /// <param name="itemLines">The collection of lines that represent the start of each item. Each line must not be null and should contain
        /// non-whitespace text.</param>
        /// <returns>An enumerable collection of item content blocks, each corresponding to an item line. Returns an empty
        /// collection if no valid content blocks are found.</returns>
        public static IEnumerable<ItemContentBlockDto> GetItemContent(IEnumerable<ExtractedLineDto> allLines, IEnumerable<ExtractedLineDto> itemLines)
        {
            if (allLines == null || itemLines == null)
                return Enumerable.Empty<ItemContentBlockDto>();

            var orderedLines = allLines
                .Where(l => l != null && !string.IsNullOrWhiteSpace(l.Text))
                .OrderBy(l => l.PageNumber)
                .ThenByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .ToList();

            var orderedItemLines = itemLines
                .Where(l => l != null && !string.IsNullOrWhiteSpace(l.Text))
                .OrderBy(l => l.PageNumber)
                .ThenByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .ToList();

            if (!orderedLines.Any() || !orderedItemLines.Any())
                return Enumerable.Empty<ItemContentBlockDto>();

            var result = new List<ItemContentBlockDto>();

            for (int i = 0; i < orderedItemLines.Count; i++)
            {
                var currentItemLine = orderedItemLines[i];
                var nextItemLine = i < orderedItemLines.Count - 1
                    ? orderedItemLines[i + 1]
                    : null;

                int startIndex = orderedLines.FindIndex(l => SameLine(l, currentItemLine));
                if (startIndex == -1)
                    continue;

                int endIndex;

                if (nextItemLine != null)
                {
                    endIndex = orderedLines.FindIndex(startIndex + 1, l => SameLine(l, nextItemLine));
                }
                else
                {
                    endIndex = orderedLines.FindIndex(startIndex + 1, l => IsEndOfItemsLine(l));
                }

                if (endIndex == -1)
                    endIndex = orderedLines.Count;

                var content = orderedLines
                    .Skip(startIndex + 1)
                    .Take(endIndex - startIndex - 1)
                    .Where(l =>
                        !ShouldIgnoreLine(l) &&
                        l.Y < 575)
                    .ToList();

                result.Add(new ItemContentBlockDto(currentItemLine, content));
            }

            return result;
        }


        /// <summary>
        /// Identifies section headings within a collection of extracted lines and groups the lines into section blocks
        /// based on those headings.
        /// </summary>
        /// <remarks>Lines are grouped into sections based on detected headings, which are determined by
        /// parsing the text of each line. The input lines are ordered by page number, vertical position, and horizontal
        /// position before processing. This method is typically used to extract structured sections from documents such
        /// as PDFs.</remarks>
        /// <param name="lines">The collection of extracted lines to analyze for section headings and content. Cannot be null.</param>
        /// <returns>An enumerable collection of section blocks, each containing a section heading and its associated lines.
        /// Returns an empty collection if no section headings are found or if the input is null.</returns>
        public static IEnumerable<SectionBlockDto> GetSectionBlocksFromLines(IEnumerable<ExtractedLineDto> lines)
        {
            if (lines == null)
                return Enumerable.Empty<SectionBlockDto>();

            var ordered = lines
                .OrderBy(l => l.PageNumber)
                .ThenByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .ToList();

            var headings = ordered
                .Select((line, index) => new
                {
                    Index = index,
                    Section = TryParseSectionFromLine(line.Text)
                })
                .Where(x => x.Section != null)
                .Select(x => new
                {
                    x.Index,
                    Section = x.Section!
                })
                .ToList();

            if (headings.Count == 0)
                return Enumerable.Empty<SectionBlockDto>();

            var result = new List<SectionBlockDto>();

            for (int i = 0; i < headings.Count; i++)
            {
                int startIndex = headings[i].Index;
                int endIndex = (i < headings.Count - 1)
                    ? headings[i + 1].Index - 1
                    : ordered.Count - 1;

                var blockLines = ordered
                    .Skip(startIndex)
                    .Take(endIndex - startIndex + 1)
                    .ToList();

                result.Add(new SectionBlockDto(
                    headings[i].Section,
                    blockLines));
            }

            return result;
        }

        /// <summary>
        /// Separates the provided lines into content lines and bullet point lines, excluding the first line.
        /// </summary>
        /// <param name="blockLines">The list of lines to process. The first line is ignored; subsequent lines are classified as either content
        /// or bullet points based on their format. Cannot be null.</param>
        /// <returns>A tuple containing two lists: the first list includes content lines, and the second list includes bullet
        /// point lines. Both lists may be empty if no matching lines are found.</returns>
        private static (List<string> Content, List<string> BulletPoints) SplitContentAndBullets(List<ExtractedLineDto> blockLines)
        {
            var content = new List<string>();
            var bulletPoints = new List<string>();

            foreach (var line in blockLines.Skip(1))
            {
                var text = line.Text?.Trim();

                if (string.IsNullOrWhiteSpace(text))
                    continue;

                if (IsBullet(text))
                    bulletPoints.Add(text);
                else
                    content.Add(text);
            }

            return (content, bulletPoints);
        }

        /// <summary>
        /// Determines whether the specified child section is a direct child of the given parent section.
        /// </summary>
        /// <remarks>A direct child is defined as a section whose name starts with the parent section
        /// followed by a dot, and has exactly one additional hierarchical level.</remarks>
        /// <param name="parentSection">The name of the parent section, using dot notation to indicate hierarchy. Cannot be null.</param>
        /// <param name="childSection">The name of the child section to evaluate, using dot notation to indicate hierarchy. Cannot be null.</param>
        /// <returns>true if the child section is a direct child of the parent section; otherwise, false.</returns>
        private static bool IsDirectChild(string parentSection, string childSection)
        {
            if (!childSection.StartsWith(parentSection + "."))
                return false;

            int parentLevel = parentSection.Count(c => c == '.') + 1;
            int childLevel = childSection.Count(c => c == '.') + 1;

            return childLevel == parentLevel + 1;
        }

        /// <summary>
        /// Builds a hierarchical representation of a purchase order overhead section, including its content, bullet
        /// points, and any nested subsections.
        /// </summary>
        /// <remarks>This method recursively processes the provided section blocks to construct a tree
        /// structure reflecting the document's hierarchy. Only direct child sections of the current section are
        /// included as subsections.</remarks>
        /// <param name="current">The section block representing the current section to process. Cannot be null.</param>
        /// <param name="allBlocks">A list of all section blocks available for constructing the hierarchy. Cannot be null and must contain the
        /// current section.</param>
        /// <returns>A PurchaseOrderOverhead object representing the current section and its nested subsections, with associated
        /// content and bullet points.</returns>
        private static PurchaseOrderOverhead BuildPurchaseOrderOverhead(SectionBlockDto current, List<SectionBlockDto> allBlocks)
        {
            var childBlocks = allBlocks
                .Where(b => IsDirectChild(current.Section.SectionNumber, b.Section.SectionNumber))
                .ToList();

            var subSections = childBlocks
                .Select(child => BuildPurchaseOrderOverhead(child, allBlocks))
                .ToList();

            var ownBodyLines = current.BlockLines
                .Skip(1) // skip heading line
                .Where(line =>
                {
                    var parsed = TryParseSectionFromLine(line.Text);
                    return parsed == null || parsed.Level <= current.Section.Level;
                })
                .ToList();

            var split = SplitContentAndBullets(ownBodyLines.Prepend(current.BlockLines.First()).ToList());

            return new PurchaseOrderOverhead(
                current.BlockLines.Select(l => l.PageNumber).Distinct().ToHashSet(),
                current.Section.SectionNumber,
                current.Section.Title,
                split.Content,
                split.BulletPoints,
                subSections);
        }
    }
}
