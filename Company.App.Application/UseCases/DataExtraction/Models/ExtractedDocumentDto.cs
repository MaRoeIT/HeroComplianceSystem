namespace Company.App.Application.UseCases.DataExtraction.Models
{
    /// <summary>
    /// Represents the extracted textual content of a PDF document,
    /// including both word-level and line-level information.
    /// </summary>
    public record ExtractedDocumentDto
    {
        /// <summary>
        /// All extracted words from the document, including position data.
        /// </summary>
        public List<ExtractedWordDto> Words { get; set; } = new();
        
        /// <summary>
        /// All extracted lines from the document, including position data.
        /// </summary>
        public List<ExtractedLineDto> Lines { get; set; } = new();
    }
}