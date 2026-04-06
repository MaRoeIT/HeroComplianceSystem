using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea
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
            var firstPageLines = GetLinesOnPage(lines, 1);
            
            var orderNumber = GetValueByLineAndPattern(firstPageLines, "PO No", 1, 8, 12);
            var orderDate = GetValueByLineAndPattern(firstPageLines, "Date Created", 4);
            
            var header = MapHeader(document);
            var overhead = MapOverhead(document);
            var items = MapItems(document);

            int pageNumbers = GetNumbersofPagesInFile(lines);
            var netAndTotalPage = GetLinesOnPage(lines, pageNumbers - 1);

            var linesNet = GetLinesFromTargetLine(netAndTotalPage, "Net Value", 2);
            var totalNetValue = GetValueByLineAndPattern(linesNet, "Net Value", 2);

            var linesTotal = GetLinesFromTargetLine(netAndTotalPage, "Total Amount", 2);
            var totalAmount = GetValueByLineAndPattern(linesTotal, "Total Amount", 2);

            return new PurchaseOrder(
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

            var seller = MapSeller(document);
            var buyer = MapBuyer(document);
            var vendorAddress = MapVendorAddress(document);
            var invoiceAddress = MapInvoiceAddress(document);
            var deliveryAddress = MapDeliveryAddress(document);

            return new PurchaseOrderHeader(
                seller,
                buyer,
                vendorAddress,
                invoiceAddress,
                deliveryAddress);
        }

        public Seller MapSeller(ExtractedDocumentDto document)
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

        public Buyer MapBuyer(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var firstPageLines = GetLinesOnPage(lines, 1);

            var revisionNumber = GetValueByLineAndPattern(firstPageLines, "Rev No", 1, 1, 3);

            var dateCreated = GetValueByLineAndPattern(firstPageLines, "Date Created", 4);

            var currency = GetValueByLineAndPattern(firstPageLines, "Currency", 5, start: "Currency", end: "$");

            var contactPerson = GetValueByLineAndPattern(firstPageLines, "Buyer/Phone", 5, start: "Buyer/Phone", end: "$");

            var confirmationFax = GetValueByLineAndPattern(firstPageLines, "Confirmation fax", 5, start: "Confirmation fax", end: "$");

            var frameAgreement = GetValueByLineAndPattern(firstPageLines, "Our Reference", 5, start: "Our Reference", end: "$");
            
            var paymentTerms = GetValueByLineAndPattern(firstPageLines, "Payment terms", 5, start: "Payment terms", end: "$");

            var technicalContact = GetValueByLineAndPattern(firstPageLines, "Technical Contact", 5, start: "Technical Contact", end: "$");

            var qSResponsible = GetValueByLineAndPattern(firstPageLines, "QS Responsible", 5, start: "QS Responsible", end: "$");

            return new Buyer(
                revisionNumber,
                dateCreated,
                currency,
                contactPerson,
                confirmationFax,
                frameAgreement,
                paymentTerms,
                technicalContact,
                qSResponsible);
        }

        public Address MapVendorAddress(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var words = document.Words;
            var firstPageLines = GetLinesOnPage(lines, 1);
            var vendorSection = GetLinesFromTargetLine(firstPageLines, "Vendor Address", 3);

            var companies = GetCompaniesListedFromLine(vendorSection.FirstOrDefault());
            var companyName = companies.FirstOrDefault();

            var streetName = GetPartOfLineRelativeToX(words, vendorSection.Skip(1).FirstOrDefault(), 30, 200);
            RemoveNumbersFromString(streetName);

            var streetNumber = GetPartOfLineRelativeToX(words, vendorSection.Skip(1).FirstOrDefault(), 30, 200);
            RemoveCharFromString(streetNumber);


            var postalNumber = GetNthWordInString(vendorSection.Skip(2).Select(l => l.Text).FirstOrDefault(), 1);

            var city = GetNthWordInString(vendorSection.Skip(2).Select(l => l.Text).FirstOrDefault(), 2);

            var country = string.Empty;

            return new Address(
                companyName,
                streetName,
                streetNumber,
                postalNumber,
                city,
                country);
        }

        public Address MapInvoiceAddress(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var words = document.Words;
            var firstPageLines = GetLinesOnPage(lines, 1);
            var vendorSection = GetLinesFromTargetLine(firstPageLines, "Invoice Address", 4);

            var companies = GetCompaniesListedFromLine(vendorSection.FirstOrDefault());
            var companyName = companies.LastOrDefault();

            var streetName = RemoveNumbersFromString(GetPartOfLineRelativeToX(words, vendorSection.Skip(1).FirstOrDefault(), 300, 600));

            var streetNumber = RemoveCharFromString(GetPartOfLineRelativeToX(words, vendorSection.Skip(1).FirstOrDefault(), 300, 600));

            var postalNumber = RemoveCharFromString(GetPartOfLineRelativeToX(words, vendorSection.Skip(2).FirstOrDefault(), 300, 450));

            var city = RemoveNumbersFromString(GetPartOfLineRelativeToX(words, vendorSection.Skip(2).FirstOrDefault(), 300, 600), "-");

            var country = GetPartOfLineRelativeToX(words, vendorSection.Skip(3).FirstOrDefault(), 300, 600);

            return new Address(
                companyName,
                streetName,
                streetNumber,
                postalNumber,
                city,
                country);
        }

        public DeliveryAddress MapDeliveryAddress(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var words = document.Words;
            var firstPageLines = GetLinesOnPage(lines, 1);
            var deliverySection = GetLinesFromTargetLine(firstPageLines, "Delivery Address", 7);

            var companyName = deliverySection.Skip(1).Select(l => l.Text).FirstOrDefault();
            
            var streetName = RemoveNumbersFromString(deliverySection.Skip(2).Select(l => l.Text).FirstOrDefault());
            
            var streetNumber = RemoveCharFromString(deliverySection.Skip(2).Select(l => l.Text).FirstOrDefault());

            var postalNumber = RemoveCharFromString(deliverySection.Skip(5).Select(l => l.Text).FirstOrDefault());

            var city = RemoveNumbersFromString(deliverySection.Skip(4).Select(l => l.Text).FirstOrDefault(), "-");

            var country = deliverySection.Skip(6).Select(l => l.Text).FirstOrDefault();

            var customerType = deliverySection.Select(l => l.Text).FirstOrDefault();

            return new DeliveryAddress(
                companyName,
                streetName,
                streetNumber,
                postalNumber,
                city,
                country,
                customerType);
        }

        public PurchaseOrderOverhead MapPurchaseOrderOverhead(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var words = document.Words;

            IEnumerable<string> targets = new[]
            {
                "1. PURCHASE ORDER REVISIONS",
                "2. PURCHASE ORDER DOCUMENTS",
                "3. SCOPE OF WORK:",
                "3.2. DELIVERY DATE",
                "4. SPECIAL MARKING & PACKING REQUIREMENTS",
                "5. SPECIAL CONDITIONS OF PURCHASE",
                "6. SPECIAL ADMINISTRATIVE REQUIREMENTS",
                "6.2. MDRL",
                "6.3. BUYER's VAT numbers in EU for shipping to a 3rd Party",
                "7. CONTACT DETAILS FOR DOCUMENTATION AND COMMUNICATION:"
            };
            var targetPages = GetPagesByLineContent(lines, targets);
            var overHeadContent = GetLinesFromTargetLine(targetPages, "Rev Quantity Order Unit");

            var pageNumber = targetPages.Select(l => l.PageNumber).Distinct().ToList();

            var section = 

            var title = 

            var content = 

            var bulletPoints = 

            var subSections = 


            return new PurchaseOrderOverhead(
                pageNumber,
                section,
                title,
                content,
                bulletPoints,
                subSections);
        }
    }
}
