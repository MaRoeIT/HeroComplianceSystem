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
    /// Provides functionality to map extracted document data to a delivery address model.
    /// </summary>
    /// <remarks>This class implements the IAddressMapper interface for DeliveryAddress, enabling
    /// transformation of structured document data into a strongly-typed delivery address. Instances of this class are
    /// thread-safe for concurrent use.</remarks>
    public sealed class MapDeliveryAddress : IAddressMapper<DeliveryAddress>
    {
        /// <summary>
        /// Extracts and maps delivery address information from the specified extracted document.
        /// </summary>
        /// <remarks>The method assumes that the delivery address section is present on the first page of
        /// the document and follows a specific structure. If the expected lines are missing or formatted differently,
        /// some fields in the returned DeliveryAddress may be null or incomplete.</remarks>
        /// <param name="document">The extracted document containing lines and words to parse for delivery address details. Cannot be null.</param>
        /// <returns>A DeliveryAddress object populated with the extracted delivery address fields from the document.</returns>
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
