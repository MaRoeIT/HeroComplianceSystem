using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.PurchaseOrder
{
    public class AdministrativeRequirementsDocumentMapper : IDocumentMapper
    {
        public DocumentType SupportedType => DocumentType.AdministrativeRequirements;

        public object Map(ExtractedDocumentDto documnet)
        {
            return new
            {
                Message = "AR mapper reached!"
            };
        }
    }
}
