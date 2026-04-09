using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping
{
    /// <summary>
    /// Coordinates document type detection and dispatches the extracted document
    /// to the mapper that supports the detected type.
    /// </summary>
    public class DataMapperRouter : IDataMapperRouter
    {
        private readonly IDocumentTypeDecider _documentTypeDecider;
        private readonly IEnumerable<IDocumentMapper> _mappers;

        public DataMapperRouter(
            IDocumentTypeDecider documentTypeDecider, IEnumerable<IDocumentMapper> mappers)
        {
            _documentTypeDecider = documentTypeDecider;
            _mappers = mappers;
        }

        /// <summary>
        /// Routes the specified extracted document to the appropriate business document mapper and returns the mapping
        /// result.
        /// </summary>
        /// <remarks>If no mapper exists for the detected document type, the result will still include the
        /// detected type and the original document, but the mapped payload will be null. This allows callers to inspect
        /// the extracted document even when mapping is not available.</remarks>
        /// <param name="document">The extracted document to be routed and mapped. Cannot be null.</param>
        /// <returns>A result object containing the detected document type, the original extracted document, and the mapped
        /// payload if a suitable mapper is found; otherwise, the mapped payload is null.</returns>
        public DocumentMappingResultDto Route(ExtractedDocumentDto document)
        {
            // Build a lightweight header context used for type detection.
            var context = BuildHeaderContext(document);

            // Detect which type of business document this PDF appears to be.
            var documentType = _documentTypeDecider.Decide(context);

            // Resolve the mapper that supports the detected document type.
            var mapper = _mappers.FirstOrDefault(x => x.SupportedType == documentType);

            if (mapper is null)
            {
                // Return the detected type even when no mapper exists yet,
                // so the caller can still inspect the extracted document.
                return new DocumentMappingResultDto
                {
                    DocumentType = documentType,
                    ExtractedDocument = document,
                    MappedPayload = null
                };
            }

            // Execute the mapper and return the mapped payload together
            // with the extracted source document.
            return new DocumentMappingResultDto
            {
                DocumentType = documentType,
                ExtractedDocument = document,
                MappedPayload = mapper.Map(document)
            };
        }

        /// <summary>
        /// Builds a header context object from the specified extracted document, using lines from the first page.
        /// </summary>
        /// <param name="document">The extracted document from which to build the header context. Must not be null and should contain line
        /// data.</param>
        /// <returns>A DocumentHeaderContextDto containing candidate header lines from the first page of the document.</returns>
        private static DocumentHeaderContextDto BuildHeaderContext(ExtractedDocumentDto document)
        {
            var candidatLines = document.Lines
                .Where(x => x.PageNumber == 1)
                .OrderByDescending(x => x.Y)
                .Take(10)
                .ToList();

            return new DocumentHeaderContextDto
            {
                CandidateLines = candidatLines
            };
        }
    }
}
