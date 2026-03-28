using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    /// <summary>
    /// Contains the extracted line data considered relevant
    /// for identifying the type of a document.
    /// </summary>
    public class DocumentHeaderContextDto
    {
        /// <summary>
        /// Candidate lines, typically taken from the document header
        /// or first page, used during type detection.
        /// </summary>
        public List<ExtractedLineDto> CandidateLines { get; set; } = new();
    }
}
