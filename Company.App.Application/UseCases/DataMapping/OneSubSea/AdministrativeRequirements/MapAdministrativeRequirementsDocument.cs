using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.AdministrativeRequirements
{
    /// <summary>
    /// Maps extracted Administrative Requirements documents
    /// into a structured payload.
    /// </summary>
    public sealed class MapAdministrativeRequirementsDocument : IDocumentMapper
    {
        /// <summary>
        /// Gets the document type supported by this mapper.
        /// </summary>
        public DocumentType SupportedType => DocumentType.AdministrativeRequirements;

        /// <summary>
        /// Maps the specified extracted document to an object representing the mapped result.
        /// </summary>
        /// <remarks>This method currently returns a placeholder object. The mapping logic will be
        /// implemented in a future version.</remarks>
        /// <param name="document">The extracted document to be mapped. Cannot be null.</param>
        /// <returns>An object containing the result of the mapping operation.</returns>
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
