using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;

namespace Company.App.Application.UseCases.DataMapping.PurchaseOrder
{
    /// <summary>
    /// Maps extracted Purchase Order documents
    /// into a structured payload.
    /// </summary>
    public class PurchaseOrderDocumentMapper : IDocumentMapper
    {
        // Gets the document type supported by this mapper.
        public DocumentType SupportedType => DocumentType.PurchaseOrder;

        public object Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;

            var orderNumber = GetNumericValueByLengthFromSingleLineByKeyWordByPage(
                lines, "PO No", 1, 8, 10);
            var orderDate = GetDateValueByDDdotMMdotYYYYByPage(lines, "Date Created", 1);
            var header = MapHeader(lines);
            var overhead = MapOverhead(lines);
            var items = MapItems(lines, words);

       
            int pageNumbers = GetNumbersofPagesInFile(lines);
            var linesTotal = GetLinesFromTargetLineByPage(lines, "Total Amount", pageNumbers - 1, 2);
            var linesNet = GetLinesFromTargetLineByPage(lines, "Net Value", pageNumbers - 1, 1);

            var totalNetValue = GetPriceFormatValueFromLinesByRegEx(linesTotal);
            var totalAmount = GetPriceFormatValueFromLinesByRegEx(linesNet);

            return new Company.App.Domain.Entities.OneSubSea.PurchaseOrder(
                orderNumber,
                orderDate,
                header,
                overhead,
                items,
                totalNetValue,
                totalAmount);
        }

        public object MapHeader(ExtractedDocumentDto document)
        {
            var lines = document.Lines;

            var seller = MapSeller(lines);
            var buyer = MapBuyer(lines);
            var vendorAddress = MapAddress(lines);
            var invoiceAddress = MapAddress(lines);
            var deliveryAddress = MapDeliveryAddress(lines);
        }

        public object MapSeller(ExtractedDocumentDto document)
        {
            var lines = document.Lines;

            var purchaseOrderNumber = GetNumericValueByLengthFromSingleLineByKeyWordByPage(
                lines, "PO No", 1, 8, 10);

            var vendorNumber = GetNumericValueByLengthFromSingleLineByKeyWordByPage(
                lines, "Vendor No", 1, 5, 8);

            var faxNumber = "+" + GetNumericValueByLengthFromSingleLineByKeyWordByPage(
                lines, "Vendor Fax No", 1, 8, 12);

            var email =

            var contactPerson =

            var frameAgreement =

            var incoTerms =

            var incoTermsDescription =

            var majorMinor = 

            var intercompany = 

        }
    }
}
