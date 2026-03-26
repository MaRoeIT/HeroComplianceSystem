using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.PurchaseOrder
{
    public class PurchaseOrderDocumentMapper : IDocumentMapper
    {
        public DocumentType SupportedType => DocumentType.PurchaseOrder;

        public object Map(ExtractedDocumentDto documnet)
        {
            return new
            {
                Message = "PO mapper reached!"
            };
        }
    }
}
