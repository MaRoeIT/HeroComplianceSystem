using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.PurchaseOrder
{
    /// <summary>
    /// Maps extracted Purchase Order documents
    /// into a structured payload.
    /// </summary>
    public class PurchaseOrderDocumentMapper : IDocumentMapper
    {
        // Gets the document type supported by this mapper.
        public DocumentType SupportedType => DocumentType.PurchaseOrder;

        public object Map(ExtractedDocumentDto document)
        {
            // Placeholder mapping result until full PO mapping logic is implemented.
            return new
            {
                Message = "PO mapper reached!"
            };
        }
    }
}
