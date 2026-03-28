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
                    PayLoad = null
                };
            }

            // Execute the mapper and return the mapped payload together
            // with the extracted source document.
            return new DocumentMappingResultDto
            {
                DocumentType = documentType,
                ExtractedDocument = document,
                PayLoad = mapper.Map(document)
            };
        }

        /// <summary>
        /// Builds the header context used for document type identification.
        /// Currently only first 10 lines from the first page are considered
        /// in decending order.
        /// </summary>
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
