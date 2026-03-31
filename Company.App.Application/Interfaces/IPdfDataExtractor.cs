using System.IO;
using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.Interfaces
{
    /// <summary>
    /// Defines a service that extracts textual and positional data from PDF files.
    /// </summary>
    public interface IPdfDataExtractor
    {
        /// <summary>
        /// Extracts words and lines from a PDF document.
        /// </summary>
        /// <param name="fileData">The raw PDF file content as bytes.</param>
        /// <returns>A DTO containing extracted words and lines.</returns>
        Task<ExtractedDocumentDto> ExtractPdfData(byte[] fileData);
    }
}
