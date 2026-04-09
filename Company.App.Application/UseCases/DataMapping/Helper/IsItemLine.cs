using Company.App.Application.UseCases.DataExtraction.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Company.App.Application.UseCases.DataMapping.Helper
{
    /// <summary>
    /// Provides static methods for identifying and processing item lines and headers in text, typically for document
    /// parsing scenarios.
    /// </summary>
    /// <remarks>This class is intended for use in scenarios where lines of text need to be classified as item
    /// headers, item lines, or ignored based on specific formatting patterns. All methods are static and
    /// thread-safe.</remarks>
    public class IsItemLine
    {
        /// <summary>
        /// Determines whether the specified line matches the expected item header format.
        /// </summary>
        /// <remarks>Use this method to identify lines that represent item headers in structured text
        /// data, such as exported tables or reports. The expected format includes specific column names and
        /// spacing.</remarks>
        /// <param name="line">The line of text to evaluate for the item header format. Cannot be null.</param>
        /// <returns>true if the line matches the item header format; otherwise, false.</returns>
        public static bool HasItemHeaderFormat(string line)
        {
            string pattern = @"Item\\s+Material\\/Description\\s+Rev\\s+Quantity\\s+Order\\s+Unit\\s+Net\\s+Price\\s+Net\\s+Amount\\s*$";

            var check = Regex.Match(line, pattern);

            return check.Success;
        }

        /// <summary>
        /// Determines whether the specified string matches the expected item line format.
        /// </summary>
        /// <remarks>The expected format includes specific sequences of digits, whitespace, and decimal
        /// values. This method is typically used to validate lines in structured text files or reports.</remarks>
        /// <param name="line">The input string to evaluate for the item line format. Cannot be null.</param>
        /// <returns>true if the input string matches the item line format; otherwise, false.</returns>
        public static bool HasItemLineFormat(string line)
        {
            string pattern = @"\d{2,4}\s\d{7,10}\s\d{2}\s\d{1,2}\.\d{2}\s\d\s\w{2}\s\d{1,3}(,\d{3})*\.\d{2}\s\d{1,3}(,\d{3})*\.\d{2}";

            var check = Regex.Match(line, pattern);

            return check.Success;
        }

        /// <summary>
        /// Determines whether two extracted lines are considered to be on the same line based on their page number,
        /// text, and position.
        /// </summary>
        /// <remarks>Lines are considered to be on the same line if their page numbers and text match, and
        /// their X and Y coordinates differ by less than 0.5 units.</remarks>
        /// <param name="a">The first extracted line to compare. Cannot be null.</param>
        /// <param name="b">The second extracted line to compare. Cannot be null.</param>
        /// <returns>true if both lines are on the same page, have identical text, and their positions are within a small
        /// threshold; otherwise, false.</returns>
        public static bool SameLine(ExtractedLineDto a, ExtractedLineDto b)
        {
            if (a == null || b == null)
                return false;

            return a.PageNumber == b.PageNumber
                   && a.Text == b.Text
                   && Math.Abs(a.Y - b.Y) < 0.5
                   && Math.Abs(a.X - b.X) < 0.5;
        }

        /// <summary>
        /// Determines whether the specified line should be ignored based on its content and format.
        /// </summary>
        /// <remarks>A line is ignored if it is null, empty, contains only whitespace, matches a
        /// recognized item header format, contains the phrase "Your ref" (case-insensitive), or is identified as the
        /// end of an items section.</remarks>
        /// <param name="line">The line to evaluate for exclusion. Cannot be null.</param>
        /// <returns>true if the line should be ignored; otherwise, false.</returns>
        public static bool ShouldIgnoreLine(ExtractedLineDto line)
        {
            if (line == null || string.IsNullOrWhiteSpace(line.Text))
                return true;

            string text = line.Text.Trim();

            if (IsItemLine.HasItemHeaderFormat(text))
                return true;

            if (text.Contains("Your ref", StringComparison.OrdinalIgnoreCase))
                return true;

            if (IsEndOfItemsLine(line))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the specified line represents the end of an items section based on a predefined separator
        /// pattern.
        /// </summary>
        /// <remarks>The method checks for a specific sequence of underscores to identify the end of an
        /// items section. Leading and trailing whitespace in the line's text is ignored.</remarks>
        /// <param name="line">The line to evaluate for the end-of-items separator. Can be null.</param>
        /// <returns>true if the line contains the end-of-items separator; otherwise, false.</returns>
        public static bool IsEndOfItemsLine(ExtractedLineDto line)
        {
            if (line == null || string.IsNullOrWhiteSpace(line.Text))
                return false;

            return line.Text.Trim().Contains("_________________");
        }
    }
}
