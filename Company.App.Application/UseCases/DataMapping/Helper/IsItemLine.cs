using Company.App.Application.UseCases.DataExtraction.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Company.App.Application.UseCases.DataMapping.Helper
{
    public class IsItemLine
    {
        public static bool HasItemHeaderFormat(string line)
        {
            string pattern = @"Item\\s+Material\\/Description\\s+Rev\\s+Quantity\\s+Order\\s+Unit\\s+Net\\s+Price\\s+Net\\s+Amount\\s*$";

            var check = Regex.Match(line, pattern);

            return check.Success;
        }
        public static bool HasItemLineFormat(string line)
        {
            string pattern = @"\d{2,4}\s\d{7,10}\s\d{2}\s\d{1,2}\.\d{2}\s\d\s\w{2}\s\d{1,3}(,\d{3})*\.\d{2}\s\d{1,3}(,\d{3})*\.\d{2}";

            var check = Regex.Match(line, pattern);

            return check.Success;
        }

        public static bool SameLine(ExtractedLineDto a, ExtractedLineDto b)
        {
            if (a == null || b == null)
                return false;

            return a.PageNumber == b.PageNumber
                   && a.Text == b.Text
                   && Math.Abs(a.Y - b.Y) < 0.5
                   && Math.Abs(a.X - b.X) < 0.5;
        }

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

        public static bool IsEndOfItemsLine(ExtractedLineDto line)
        {
            if (line == null || string.IsNullOrWhiteSpace(line.Text))
                return false;

            return line.Text.Trim().Contains("_________________");
        }
    }
}
