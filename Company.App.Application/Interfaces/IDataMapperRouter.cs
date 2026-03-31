using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.Interfaces
{
    /// <summary>
    /// Routes an extracted PDF document to the correct document mapper
    /// based on the detected document type.
    /// </summary>
    public interface IDataMapperRouter
    {
        /// <summary>
        /// Detects the document type and returns the mapped result.
        /// </summary>
        /// <param name="document">The extracted PDF document to route.</param>
        /// <returns>A mapping result containing detected type and mapped payload.</returns>
        DocumentMappingResultDto Route(ExtractedDocumentDto document);
    }
}
