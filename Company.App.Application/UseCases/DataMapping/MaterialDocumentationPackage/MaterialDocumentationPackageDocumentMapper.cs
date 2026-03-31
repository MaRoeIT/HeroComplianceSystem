using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.MaterialDocumentationPackage
{
    /// <summary>
    /// Maps extracted Material Documentation Package documents
    /// into a structured payload.
    /// </summary>
    public class MaterialDocumentationPackageDocumentMapper : IDocumentMapper
    {
        /// <summary>
        /// Gets the document type supported by this mapper.
        /// </summary>
        public DocumentType SupportedType => DocumentType.MaterialDocumentationPackage;

        public object Map(ExtractedDocumentDto document)
        {
            // Placeholder mapping result until full MDP mapping logic is implemented.
            return new
            {
                Message = "MDP mapper reached!"
            };
        }
    }
}
