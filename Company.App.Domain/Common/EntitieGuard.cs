using System.Text.RegularExpressions;

namespace Company.App.Domain.Common
{
    public static class EntitieGuard
    {
        public static string Required(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} cannot be empty");

            return value;
        }
        // usage: OrderNumber = Guard.Required(orderNumber, nameof(OrderNumber));

        public static string Required(this string? value, string fieldName, string context)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Missing {fieldName} in {context}");

            return value;
        }
        // usage: OrderNumber = orderNumber.Required(nameof(OrderNumber), "Document Type");

        public static string MustBeNumeric(string value, string fieldName)
        {
            bool isNumeric = Regex.IsMatch(value, @"^\d+$");

            if (!isNumeric)
                throw new ArgumentException($"{fieldName} must be numeric");

            return value;
        }
        

    }
}
