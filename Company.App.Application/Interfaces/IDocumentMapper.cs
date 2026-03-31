using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.Interfaces
{
    /// <summary>
    /// Defines a mapper that can transform an extracted document
    /// into a structured payload for one supported document type.
    /// </summary>
    public interface IDocumentMapper
    {
        // Gets the document type supported by this mapper.
        DocumentType SupportedType { get; }
        
        /// <summary>
        /// Maps raw extracted PDF content into a structured result object.
        /// </summary>
        /// <param name="document">The extracted PDF document.</param>
        /// <returns>A mapped payload object for the supported document type.</returns>
        object Map(ExtractedDocumentDto document);
    }
}
