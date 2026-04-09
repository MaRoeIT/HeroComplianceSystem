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
    /// Provides functionality to map extracted document data to a purchase order header using configurable mappers for
    /// seller, buyer, and address information.
    /// </summary>
    /// <remarks>This class is intended for scenarios where purchase order header information must be
    /// constructed from a document extraction process. It composes multiple mapper dependencies to transform document
    /// data into a structured header object. Instances of this class are immutable and thread-safe if the provided
    /// mappers are also thread-safe.</remarks>
    public sealed class MapHeader : IPurchaseOrderHeaderMapper
    {
        private readonly ISellerMapper _sellerMapper;
        private readonly IBuyerMapper _buyerMapper;
        private readonly IAddressMapper<Address> _vendorAddressMapper;
        private readonly IAddressMapper<Address> _invoiceAddressMapper;
        private readonly IAddressMapper<DeliveryAddress> _deliveryAddressMapper;

        /// <summary>
        /// Initializes a new instance of the MapHeader class with the specified mappers for seller, buyer, vendor
        /// address, invoice address, and delivery address.
        /// </summary>
        /// <param name="sellerMapper">The mapper used to map seller information. Cannot be null.</param>
        /// <param name="buyerMapper">The mapper used to map buyer information. Cannot be null.</param>
        /// <param name="vendorAddressMapper">The mapper used to map vendor address information. Cannot be null.</param>
        /// <param name="invoiceaddressMapper">The mapper used to map invoice address information. Cannot be null.</param>
        /// <param name="deliveryAddressMapper">The mapper used to map delivery address information. Cannot be null.</param>
        public MapHeader(ISellerMapper sellerMapper, IBuyerMapper buyerMapper, IAddressMapper<Address> vendorAddressMapper, IAddressMapper<Address> invoiceaddressMapper, IAddressMapper<DeliveryAddress> deliveryAddressMapper)
        {
            _sellerMapper = sellerMapper;
            _buyerMapper = buyerMapper;
            _vendorAddressMapper = vendorAddressMapper;
            _invoiceAddressMapper = invoiceaddressMapper;
            _deliveryAddressMapper = deliveryAddressMapper;
        }

        /// <summary>
        /// Creates a new purchase order header by mapping data from the specified extracted document.
        /// </summary>
        /// <param name="document">The extracted document containing purchase order information to be mapped. Cannot be null.</param>
        /// <returns>A new instance of the purchase order header populated with data from the extracted document.</returns>
        public PurchaseOrderHeader Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;

            var seller = _sellerMapper.Map(document);
            var buyer = _buyerMapper.Map(document);
            var vendorAddress = _vendorAddressMapper.Map(document);
            var invoiceAddress = _invoiceAddressMapper.Map(document);
            var deliveryAddress = _deliveryAddressMapper.Map(document);

            return new PurchaseOrderHeader(
                seller,
                buyer,
                vendorAddress,
                invoiceAddress,
                deliveryAddress);
        }
    }
}
