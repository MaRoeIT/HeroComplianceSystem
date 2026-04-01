using System;
using System.Collections.Generic;
using System.Text;

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


    }
}
