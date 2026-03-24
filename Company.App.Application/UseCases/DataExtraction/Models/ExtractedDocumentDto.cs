
namespace Company.App.Application.UseCases.DataExtraction.Models
{
    public class ExtractedDocumentDto
    {
        public List<ExtractedWordDto> Words { get; set; } = new();
        public List<ExtractedLineDto> Lines { get; set; } = new();
    }
}
