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
    /// Provides functionality to map vendor address information from an extracted document to an Address object.
    /// </summary>
    /// <remarks>This class is intended for use when extracting and mapping vendor address details from
    /// structured document data, such as OCR or parsed invoice content. It implements the IAddressMapper<Address>
    /// interface to support consistent address mapping operations.</remarks>
    public sealed class MapVendorAddress : IVendorAddressMapper
    {
        /// <summary>
        /// Maps the extracted document data to an Address instance by parsing relevant address fields from the provided
        /// document.
        /// </summary>
        /// <remarks>This method assumes the document follows a specific structure where the vendor
        /// address appears on the first page and is labeled accordingly. If the expected structure is not present, some
        /// address fields may be incomplete or empty.</remarks>
        /// <param name="document">The extracted document data containing lines and words to be analyzed for address information. Cannot be
        /// null.</param>
        /// <returns>An Address object populated with the company name, street name, street number, postal code, city, and
        /// country parsed from the document. Fields may be empty if the corresponding information is not found.</returns>
        public Address Map(ExtractedDocumentDto document)
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
    }
}
