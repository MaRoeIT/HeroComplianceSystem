using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.AdministrativeRequirements
{
    /// <summary>
    /// Maps extracted Administrative Requirements documents
    /// into a structured payload.
    /// </summary>
    public class AdministrativeRequirementsDocumentMapper : IDocumentMapper
    {
        /// <summary>
        /// Gets the document type supported by this mapper.
        /// </summary>
        public DocumentType SupportedType => DocumentType.AdministrativeRequirements;

        public object Map(ExtractedDocumentDto document)
        {
            // Placeholder mapping result until full AR mapping logic is implemented.
            return new
            {
                Message = "AR mapper reached!"
            };
        }
    }
}
