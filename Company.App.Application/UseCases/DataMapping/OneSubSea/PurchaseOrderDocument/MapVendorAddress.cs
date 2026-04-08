using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    public sealed class MapVendorAddress : IAddressMapper<Address>
    {
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
