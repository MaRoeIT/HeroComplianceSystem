using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea
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

        /// <summary>
        /// Maps the specified extracted document to a domain-specific object.
        /// </summary>
        /// <param name="document">The extracted document data to be mapped. Cannot be null.</param>
        /// <returns>An object representing the mapped result of the extracted document.</returns>
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
