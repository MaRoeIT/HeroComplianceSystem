using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Company.App.Application.UseCases.DataMapping.Helper
{
    public class IsPriceFormatValues
    {
        public static bool HasPriceFormatValues(string line, int minCount = 1)
        {
            string pattern = @"\d{1,3}(,\d{3})*\.\d{2}";
            return Regex.Matches(line, pattern).Count >= minCount;
        }
    }
}
