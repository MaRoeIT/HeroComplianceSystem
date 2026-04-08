using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    public sealed class MapHeader : IPurchaseOrderHeaderMapper
    {
        private readonly ISellerMapper _sellerMapper;
        private readonly IBuyerMapper _buyerMapper;
        private readonly IAddressMapper<Address> _vendorAddressMapper;
        private readonly IAddressMapper<Address> _invoiceAddressMapper;
        private readonly IAddressMapper<DeliveryAddress> _deliveryAddressMapper;

        public MapHeader(ISellerMapper sellerMapper, IBuyerMapper buyerMapper, IAddressMapper<Address> vendorAddressMapper, IAddressMapper<Address> invoiceaddressMapper, IAddressMapper<DeliveryAddress> deliveryAddressMapper)
        {
            _sellerMapper = sellerMapper;
            _buyerMapper = buyerMapper;
            _vendorAddressMapper = vendorAddressMapper;
            _invoiceAddressMapper = invoiceaddressMapper;
            _deliveryAddressMapper = deliveryAddressMapper;
        }

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
