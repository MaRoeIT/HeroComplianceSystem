using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    /// <summary>
    /// Represents a request to map an already extracted document
    /// into a typed business payload.
    /// </summary>
    public class DocumentMappingRequestDto
    {
        // The extracted document to be mapped.
        public ExtractedDocumentDto DocumentDto { get; set; } = new();
    }
}
