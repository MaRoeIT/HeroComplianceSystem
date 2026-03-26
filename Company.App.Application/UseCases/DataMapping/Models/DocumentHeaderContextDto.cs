using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    public class DocumentHeaderContextDto
    {
        public List<ExtractedLineDto> CandidateLines { get; set; } = new();
    }
}
