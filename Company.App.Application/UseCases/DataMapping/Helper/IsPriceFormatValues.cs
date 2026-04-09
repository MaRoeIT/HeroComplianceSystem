using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Company.App.Application.UseCases.DataMapping.Helper
{
    /// <summary>
    /// Provides methods for detecting price-formatted values within strings.
    /// </summary>
    public class IsPriceFormatValues
    {
        /// <summary>
        /// Determines whether the specified string contains at least a given number of values in a standard price
        /// format.
        /// </summary>
        /// <remarks>A price-formatted value is defined as a number with optional thousands separators and
        /// exactly two decimal places (e.g., "1,234.56").</remarks>
        /// <param name="line">The input string to search for price-formatted values.</param>
        /// <param name="minCount">The minimum number of price-formatted values that must be present for the method to return <see
        /// langword="true"/>. Must be greater than or equal to 1. The default is 1.</param>
        /// <returns>true if the input string contains at least the specified number of price-formatted values; otherwise, false.</returns>
        public static bool HasPriceFormatValues(string line, int minCount = 1)
        {
            string pattern = @"\d{1,3}(,\d{3})*\.\d{2}";
            return Regex.Matches(line, pattern).Count >= minCount;
        }
    }
}
