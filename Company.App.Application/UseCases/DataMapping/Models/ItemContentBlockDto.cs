using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    public sealed record ItemContentBlockDto
    (
        ExtractedLineDto ItemLine,
        IEnumerable<ExtractedLineDto> ContentLines

    );
}
