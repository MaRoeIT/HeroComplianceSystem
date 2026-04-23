using Company.App.Application.Interfaces.OneSubSea.PurchaseOrderDocument;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    /// <summary>
    /// Provides functionality to map invoice address information from an extracted document to an Address object.
    /// </summary>
    /// <remarks>This class is intended for use in scenarios where invoice address details need to be parsed
    /// and structured from document extraction results. It implements the IAddressMapper<Address> interface to support
    /// consistent address mapping operations.</remarks>
    public sealed class MapInvoiceAddress : IInvoiceAddressMapper
    {
        /// <summary>
        /// Extracts and maps address information from the specified extracted document data transfer object.
        /// </summary>
        /// <remarks>The mapping relies on the presence and structure of lines labeled as 'Invoice
        /// Address' within the first page of the document. If the expected structure is not present, some address
        /// fields may be empty or incomplete.</remarks>
        /// <param name="document">The extracted document containing lines and words from which address information is parsed. Cannot be null.</param>
        /// <returns>An Address object containing the company name, street name, street number, postal number, city, and country
        /// parsed from the document.</returns>
        public Address Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var words = document.Words;
            var firstPageLines = GetLinesOnPage(lines, 1);
            var targetSection = GetLinesFromTargetLine(firstPageLines, "Invoice Address", 4);
            var vendorSection = GetPartOfLinesRelativeToX(words, targetSection, 300, 600);

            var companyName = vendorSection.First().Text;

            var streetName = RemoveNumbersFromString(vendorSection.ElementAt(1).Text);

            var streetNumber = RemoveCharFromString(vendorSection.ElementAt(1).Text);

            var postalNumber = GetNthWordInString(vendorSection.ElementAt(2).Text, 1);

            var city = RemoveSymbolsFromString(RemoveNumbersFromString(vendorSection.ElementAt(2).Text));

            var country = vendorSection.ElementAt(3).Text;

            return new Address(
                companyName,
                streetName,
                streetNumber,
                postalNumber,
                city,
                country);
        }
    }
}
