using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.PurchaseOrder
{
    public class MaterialDocumentationPackageDocumentMapper : IDocumentMapper
    {
        public DocumentType SupportedType => DocumentType.MaterialDocumentationPackage;

        public object Map(ExtractedDocumentDto documnet)
        {
            return new
            {
                Message = "MDP mapper reached!"
            };
        }
    }
}
