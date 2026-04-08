using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    public sealed class MapDeliveryAddress : IAddressMapper<DeliveryAddress>
    {
        public DeliveryAddress Map(ExtractedDocumentDto document)
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
    }
}
