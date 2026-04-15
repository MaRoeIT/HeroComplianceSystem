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
    /// Provides functionality to map extracted document data to a buyer domain model.
    /// </summary>
    /// <remarks>This class implements the IBuyerMapper interface to convert structured document data into a
    /// Buyer instance. It is intended for use when processing documents where buyer information must be extracted and
    /// mapped to a strongly-typed object. This class is sealed and cannot be inherited.</remarks>
    public sealed class MapBuyer : IBuyerMapper
    {
        /// <summary>
        /// Creates a new Buyer instance by extracting relevant information from the specified document.
        /// </summary>
        /// <remarks>The method parses specific fields from the first page of the document based on
        /// predefined patterns. Ensure that the document contains the expected structure for accurate extraction of
        /// buyer information.</remarks>
        /// <param name="document">The extracted document data containing lines to be parsed for buyer information. Cannot be null.</param>
        /// <returns>A Buyer object populated with values parsed from the document. The returned object contains buyer details
        /// such as revision number, creation date, currency, contact information, and other related fields.</returns>
        public Buyer Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var firstPageLines = GetLinesOnPage(lines, 1);

            var revisionNumber = GetValueAfterLabel(firstPageLines, "Rev No");

            var dateCreated = GetValueByLineAndPattern(firstPageLines, "Date Created", 4);

            var currency = GetValueAfterLabel(firstPageLines, "Currency");

            var contactPerson = RemoveSymbolsFromString(GetValueAfterLabel(firstPageLines, "Buyer/Phone"));

            var confirmationFax = RemoveSymbolsFromString(GetValueAfterLabel(firstPageLines, "Confirmation fax"));

            var frameAgreement = GetValueAfterLabel(firstPageLines, "Our Reference");

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
