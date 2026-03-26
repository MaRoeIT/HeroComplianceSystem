using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    public class DocumentMappingResultDto
    {
        public DocumentType DocumentType { get; set; }
        public ExtractedDocumentDto? ExtractedDocument { get; set; }
        public object? PayLoad { get; set; }
    }
}
