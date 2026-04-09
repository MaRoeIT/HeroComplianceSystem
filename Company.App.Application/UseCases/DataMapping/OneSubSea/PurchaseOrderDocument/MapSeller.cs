using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    /// <summary>
    /// Provides functionality to map extracted document data to a seller domain model.
    /// </summary>
    /// <remarks>This class implements the ISellerMapper interface to extract and transform relevant seller
    /// information from a structured document. It is typically used in scenarios where seller details must be parsed
    /// from document data for further processing or integration.</remarks>
    public class MapSeller : ISellerMapper
    {
        /// <summary>
        /// Creates a new Seller instance by extracting relevant seller information from the specified extracted
        /// document.
        /// </summary>
        /// <remarks>This method extracts seller-related fields such as purchase order number, vendor
        /// number, contact details, and other attributes from the first page of the document. The extraction relies on
        /// specific patterns and line positions within the document's text. If required fields are missing or formatted
        /// unexpectedly, the resulting Seller object may contain empty or partial data.</remarks>
        /// <param name="document">The extracted document containing lines of text from which seller information will be parsed. Cannot be
        /// null.</param>
        /// <returns>A Seller object populated with values parsed from the provided document.</returns>
        public Seller Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var firstPageLines = GetLinesOnPage(lines, 1);

            var purchaseOrderNumber = GetValueByLineAndPattern(firstPageLines, "PO No", 1, 8, 12);

            var vendorNumber = GetValueByLineAndPattern(firstPageLines, "Vendor No", 1, 5, 10);

            var faxNumber = "+" + GetValueByLineAndPattern(firstPageLines, "Vendor Fax No", 1, 9, 15);

            var email = GetValueByLineAndPattern(firstPageLines, "Vendor email", 3);

            var vendorContact = GetValueByLineAndPattern(firstPageLines, "Vendor Contact", 5, start: "Vendor Contact", end: "Confirmation");

            var frameAgreement = GetValueByLineAndPattern(firstPageLines, "Your ref", 5, start: "Your ref", end: "Our");

            var incoTerms = GetValueByLineAndPattern(firstPageLines, "Inco Terms", 5, start: "Inco Terms", end: "Payment");

            var incoTermsDescription = GetValueByLineAndPattern(firstPageLines, "Incoterms desc", 5, start: "Incoterms desc", end: "Technical");

            var majorMinor = GetValueByLineAndPattern(firstPageLines, "Major/Minor PO", 5, start: "Major/Minor PO", end: "QS");

            var intercompany = GetNthWordInString(GetFirstLineContaining(firstPageLines, "Intercompany PO").Text, -1);

            return new Seller(
                purchaseOrderNumber,
                vendorNumber,
                faxNumber,
                email,
                vendorContact,
                frameAgreement,
                incoTerms,
                incoTermsDescription,
                majorMinor,
                intercompany);
        }
    }
}
