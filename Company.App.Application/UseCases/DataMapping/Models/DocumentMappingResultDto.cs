using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    /// <summary>
    /// Represents the outcome of document type detection and mapping.
    /// </summary>
    public class DocumentMappingResultDto
    {
        // The detected type of the document.
        public DocumentType DocumentType { get; set; }

        // The original extracted document used as input for mapping.
        // Kept for debugging, inspection, and UI display.
        public ExtractedDocumentDto? ExtractedDocument { get; set; }

        // The mapped business payload produced by the selected document mapper.
        public object? MappedPayload { get; set; }
    }
}
