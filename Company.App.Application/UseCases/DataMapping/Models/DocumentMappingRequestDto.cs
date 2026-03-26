using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    public class DocumentMappingRequestDto
    {
        public ExtractedDocumentDto DocumentDto { get; set; } = new();
    }
}
