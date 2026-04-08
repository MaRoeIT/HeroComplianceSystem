using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    public sealed class MapInvoiceAddress : IAddressMapper<Address>
    {
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
