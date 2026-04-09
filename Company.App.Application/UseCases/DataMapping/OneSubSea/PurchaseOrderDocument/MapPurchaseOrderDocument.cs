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
    /// Maps extracted Purchase Order documents
    /// into a structured payload.
    /// </summary>
    public class MapPurchaseOrderDocument : IDocumentMapper
    {
        // Gets the document type supported by this mapper.
        public DocumentType SupportedType => DocumentType.PurchaseOrder;

        private readonly IPurchaseOrderHeaderMapper _headerMapper;
        private readonly IPurchaseOrderOverheadMapper _overheadMapper;
        private readonly IItemMapper _itemMapper;

        /// <summary>
        /// Initializes a new instance of the MapPurchaseOrderDocument class with the specified mappers for purchase
        /// order headers, overheads, and items.
        /// </summary>
        /// <param name="headerMapper">The mapper used to transform purchase order header data.</param>
        /// <param name="overheadMapper">The mapper used to transform purchase order overhead data.</param>
        /// <param name="itemMapper">The mapper used to transform purchase order item data.</param>
        public MapPurchaseOrderDocument(IPurchaseOrderHeaderMapper headerMapper, IPurchaseOrderOverheadMapper overheadMapper, IItemMapper itemMapper)
        {
            _headerMapper = headerMapper;
            _overheadMapper = overheadMapper;
            _itemMapper = itemMapper;
        }

        /// <summary>
        /// Maps the specified extracted document data transfer object to a strongly typed purchase order object.
        /// </summary>
        /// <remarks>The mapping process extracts key purchase order information, including order number,
        /// order date, header, overhead, items, net value, and total amount, from the provided document. The returned
        /// object aggregates these values for further processing or storage.</remarks>
        /// <param name="document">The extracted document containing lines and metadata to be mapped to a purchase order. Cannot be null.</param>
        /// <returns>A purchase order object representing the mapped data from the extracted document.</returns>
        public object Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var firstPageLines = GetLinesOnPage(lines, 1);
            
            var orderNumber = GetValueByLineAndPattern(firstPageLines, "PO No", 1, 8, 12);
            var orderDate = GetValueByLineAndPattern(firstPageLines, "Date Created", 4);
            
            var header = _headerMapper.Map(document);
            var overhead = _overheadMapper.Map(document);

            var items = _itemMapper.Map(document);

            int pageNumbers = GetNumbersofPagesInFile(lines);
            var netAndTotalPage = GetLinesOnPage(lines, pageNumbers - 1);

            var linesNet = GetLinesFromTargetLine(netAndTotalPage, "Net Value", 2);
            var totalNetValue = GetValueByLineAndPattern(linesNet, "Net Value", 2);

            var linesTotal = GetLinesFromTargetLine(netAndTotalPage, "Total Amount", 2);
            var totalAmount = GetValueByLineAndPattern(linesTotal, "Total Amount", 2);

            return new PurchaseOrder(
                orderNumber,
                orderDate,
                header,
                overhead,
                items,
                totalNetValue,
                totalAmount);
        }
    }
}
