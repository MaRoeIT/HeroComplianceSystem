using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    /// <summary>
    /// Represents a content block consisting of a primary item line and its associated content lines.
    /// </summary>
    /// <param name="ItemLine">The main line that identifies the item within the content block. Cannot be null.</param>
    /// <param name="ContentLines">A collection of lines that provide additional content or details related to the item line. Cannot be null.</param>
    public sealed record ItemContentBlockDto
    (
        ExtractedLineDto ItemLine,
        IEnumerable<ExtractedLineDto> ContentLines

    );
}
