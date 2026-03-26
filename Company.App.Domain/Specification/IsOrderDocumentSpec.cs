using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Domain.Specification
{
    public class IsOrderDocumentSpec
    {
        public bool IsPurchaseOrder(string documnetTitle) => 
            documnetTitle.Contains("Purchase Order", StringComparison.OrdinalIgnoreCase);
        public bool IsMaterialDocumentationPackage(string documnetTitle) =>
            documnetTitle.Contains("Material Documentation Package", StringComparison.OrdinalIgnoreCase);
        public bool IsAdministrativeRequirements(string documnetTitle) =>
            documnetTitle.Contains("Administrative Requirements", StringComparison.OrdinalIgnoreCase);
    }
}
