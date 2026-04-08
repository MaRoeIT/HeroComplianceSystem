using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    public class MapSeller : ISellerMapper
    {
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
