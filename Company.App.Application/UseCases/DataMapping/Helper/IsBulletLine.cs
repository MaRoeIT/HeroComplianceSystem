using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.Helper
{
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
