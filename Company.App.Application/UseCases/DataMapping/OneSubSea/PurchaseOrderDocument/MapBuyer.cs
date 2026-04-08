using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    public sealed class MapBuyer : IBuyerMapper
    {

        public Buyer Map(ExtractedDocumentDto document)
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
    }
}
