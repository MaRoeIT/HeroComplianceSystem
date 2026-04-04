using System.Text.RegularExpressions;

namespace Company.App.Domain.Specification
{
    /// <summary>
    /// Encapsulates simple specification logic for identifying
    /// supported order-related document types from document titles or header text.
    /// </summary>
    public class IsOrderDocumentSpec
    {
        /// <summary>
        /// Determines whether the supplied text identifies a purchase order document.
        /// </summary>
        /// <param name="documentTitle">The title or header text to inspect.</param>
        /// <returns>True when the text matches a purchase order pattern; otherwise false.</returns>
        public bool IsPurchaseOrder(string documentTitle) => 
            documentTitle.Contains("Purchase Order", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Determines whether the supplied text identifies a material documentation package document.
        /// </summary>
        /// <param name="documentTitle">The title or header text to inspect.</param>
        /// <returns>True when the text matches a material documentation package pattern; otherwise false.</returns>
        public bool IsMaterialDocumentationPackage(string documentTitle) =>
            documentTitle.Contains("Material Documentation Package", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Determines whether the supplied text identifies an administrative requirements document.
        /// </summary>
        /// <param name="documentTitle">The title or header text to inspect.</param>
        /// <returns>True when the text matches an administrative requirements pattern; otherwise false.</returns>
        public bool IsAdministrativeRequirements(string documentTitle) =>
            documentTitle.Contains("Administrative Requirements", StringComparison.OrdinalIgnoreCase);

        public bool IsNumerig
    }
}
