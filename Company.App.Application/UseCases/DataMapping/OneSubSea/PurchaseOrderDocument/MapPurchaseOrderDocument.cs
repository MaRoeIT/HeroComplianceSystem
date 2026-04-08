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

        public MapPurchaseOrderDocument(IPurchaseOrderHeaderMapper headerMapper, IPurchaseOrderOverheadMapper overheadMapper)
        {
            _headerMapper = headerMapper;
            _overheadMapper = overheadMapper;
        }

        public object Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var firstPageLines = GetLinesOnPage(lines, 1);
            
            var orderNumber = GetValueByLineAndPattern(firstPageLines, "PO No", 1, 8, 12);
            var orderDate = GetValueByLineAndPattern(firstPageLines, "Date Created", 4);
            
            var header = _headerMapper.Map(document);
            var overhead = _overheadMapper.Map(document);
            var items = MapItems(document);

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
