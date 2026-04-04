using Company.App.Application.UseCases.DataExtraction.Models;
using System;
using System.Text.RegularExpressions;

namespace Company.App.Application.UseCases.DataMapping.Services
{
    public static class ExtractedDocumentSearch
    {
        public static ExtractedLineDto? GetFirstLineContaining(IEnumerable<ExtractedLineDto> lines, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return lines.FirstOrDefault(l =>
            l.Text.Contains(text, StringComparison.OrdinalIgnoreCase));
        }

        public static ExtractedLineDto? GetFirstLineContainingByPage(IEnumerable<ExtractedLineDto> line, string text, int pageNumber)
        {
            return line
                .Where(l => l.PageNumber == pageNumber)
                .OrderByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .FirstOrDefault(l =>
                l.Text.Contains(text, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<ExtractedLineDto> GetLinesOnPage(IEnumerable<ExtractedLineDto> lines, int pageNumber)
        {
            return lines
                .Where(l => l.PageNumber == pageNumber)
                .OrderByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .ToList();
        }


        public static IEnumerable<ExtractedWordDto> GetWordsByWidthAndPage(IEnumerable<ExtractedWordDto> words, int pageNumber, double width)
        {
            return words.Where(w =>
                w.PageNumber == pageNumber &&
                w.Width == width)
                .OrderByDescending(w => w.Y)
                .ThenBy(w => w.X)
                .ToList();
        }

        public static ExtractedWordDto? GetWordByNumberOfCharAndPage(
            IEnumerable<ExtractedWordDto> words, int pageNumber, int charInString)
        {
            return words.Where(w =>
                w.PageNumber == pageNumber &&
                w.Text.Length == charInString)
                .OrderByDescending(w => w.Y)
                .ThenBy(w => w.X)
                .FirstOrDefault();
            
        }

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

        public static string GetNumericValueinStringByLength(string text, int minLength, int maxLength)
        {
            string pattern = @"\d{" + $"{minLength}" + "," + $"{maxLength}" + "}";

            var match = Regex.Match(text, pattern);

            return match.Success ? match.Value : string.Empty;
        }

        public static string GetPriceFormatValueFromLinesByRegEx(IEnumerable<ExtractedLineDto> lines)
        {
            string pattern = @"\d{1,3}(,\d{3})*\.\d{2}";

            return lines
                .Select(l => Regex.Match(l.Text, pattern))
                .FirstOrDefault(m => m.Success)?.Value ?? string.Empty;

        }

        public static string GetEmailValueByRegEx(IEnumerable<ExtractedLineDto> lines)
        {
            string pattern = "(?<=\\s)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}(?=\\s)";

            return lines
                .Select(l => Regex.Match(l.Text, pattern))
                .FirstOrDefault(m => m.Success)?.Value ?? string.Empty;
        }


        //Combinated Methods

        public static string GetNumericValueByLengthFromSingleLineByKeyWordByPage(
            IEnumerable<ExtractedLineDto> lines,
            string textInLine,
            int pageNumber,
            int minLength,
            int maxLength)
        {
            var extractedLine = GetFirstLineContainingByPage(lines, textInLine, pageNumber);

            var value = GetNumericValueinStringByLength(extractedLine.Text, minLength, maxLength);

            return value;
        }

        public static string GetDateValueByDDdotMMdotYYYYByPage(
            IEnumerable<ExtractedLineDto> lines,
            string textInLine,
            int pageNumber)
        {
            var extractedLine = GetFirstLineContainingByPage(lines, textInLine, pageNumber);

            string pattern = @"\d\d\.\d\d\.\d\d\d\d";

            var date = Regex.Match(extractedLine.Text, pattern);

            return date.Success ? date.Value : String.Empty;
        }

        public static int GetNumbersofPagesInFile(IEnumerable<ExtractedLineDto> document)
        {
            return document.Select(d => d.PageNumber)
                .OrderByDescending(d => d)
                .FirstOrDefault();


        }

        public static IEnumerable<ExtractedLineDto> GetLinesFromTargetLineByPage(
            IEnumerable<ExtractedLineDto> lines,
            string target,
            int pageNumber,
            int followingLines)
        {
            var pageLines = lines
                .Where(l => l.PageNumber == pageNumber)
                .OrderByDescending(l => l.Y)
                .ThenBy(l => l.X)
                .ToList();

            var startIndex = pageLines.FindIndex(l => l.Text.Contains(target));

            return pageLines
                .Skip(startIndex)
                .Take(followingLines + 1) // +1 to include the target line itself
                .ToList();
        }



    }
}
