using System.Text.RegularExpressions;

namespace Company.App.Domain.Common
{
    /// <summary>
    /// Provides static guard methods for validating string values and enforcing common input requirements.
    /// </summary>
    /// <remarks>The methods in this class are intended to simplify validation of required fields and numeric
    /// input in application code. They throw exceptions when validation fails, allowing for concise and consistent
    /// input checking. These methods are typically used to validate parameters or properties before further
    /// processing.</remarks>
    public static class EntitieGuard
    {
        /// <summary>
        /// Ensures that a required string value is not null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string value to validate as required. Cannot be null, empty, or white space.</param>
        /// <param name="fieldName">The name of the field being validated. Used in the exception message if validation fails.</param>
        /// <returns>The original string value if it is not null, empty, or white space.</returns>
        /// <exception cref="ArgumentException">Thrown if the value is null, empty, or consists only of white-space characters.</exception>
        public static string Required(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} cannot be empty");

            return value;
        }
        // usage: OrderNumber = Guard.Required(orderNumber, nameof(OrderNumber));

        /// <summary>
        /// Ensures that the specified string value is not null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string value to validate. Cannot be null, empty, or white space.</param>
        /// <param name="fieldName">The name of the field being validated. Used in the exception message to identify the missing value.</param>
        /// <param name="context">A description of the context in which the validation is occurring. Used in the exception message to provide
        /// additional information.</param>
        /// <returns>The original string value if it is not null, empty, or white space.</returns>
        /// <exception cref="ArgumentException">Thrown if the value is null, empty, or consists only of white-space characters.</exception>
        public static string Required(this string? value, string fieldName, string context)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Missing {fieldName} in {context}");

            return value;
        }
        // usage: OrderNumber = orderNumber.Required(nameof(OrderNumber), "Document Type");

        /// <summary>
        /// Validates that the specified string contains only numeric characters.
        /// </summary>
        /// <param name="value">The string value to validate for numeric content.</param>
        /// <param name="fieldName">The name of the field being validated, used in exception messages to identify the source of the error.</param>
        /// <returns>The original string value if it contains only numeric characters.</returns>
        /// <exception cref="ArgumentException">Thrown if the value contains any non-numeric characters.</exception>
        public static string MustBeNumeric(string value, string fieldName)
        {
            bool isNumeric = Regex.IsMatch(value, @"^\d+$");

            if (!isNumeric)
                throw new ArgumentException($"{fieldName} must be numeric");

            return value;
        }
        

    }
}
