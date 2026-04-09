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
    /// Provides functionality to map invoice address information from an extracted document to an Address object.
    /// </summary>
    /// <remarks>This class is intended for use in scenarios where invoice address details need to be parsed
    /// and structured from document extraction results. It implements the IAddressMapper<Address> interface to support
    /// consistent address mapping operations.</remarks>
    public sealed class MapInvoiceAddress : IAddressMapper<Address>
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
    }
}
