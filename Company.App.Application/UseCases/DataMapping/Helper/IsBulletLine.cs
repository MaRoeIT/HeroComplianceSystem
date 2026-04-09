using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.Helper
{
    /// <summary>
    /// Provides utility methods for determining whether a line of text represents a bullet point.
    /// </summary>
    /// <remarks>This class is static and cannot be instantiated.</remarks>
    public static class IsBulletLine
    {
        public static bool IsBullet(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            var trimmed = text.Trim();

            return trimmed.StartsWith("o ")
                || trimmed.StartsWith("- ")
                || trimmed.StartsWith("* ")
                || trimmed.StartsWith("•");
        }
    }
}
