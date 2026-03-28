using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataExtraction
{
    /// <summary>
    /// Application use case for extracting structured text data from a PDF file.
    /// </summary>
    public class ExtractPdfUseCase
    {
        private readonly IPdfDataExtractor _scanner;

        public ExtractPdfUseCase(IPdfDataExtractor scanner)
        {
            _scanner = scanner;
        }

        /// <summary>
        /// Executes PDF extraction for the provided file data.
        /// </summary>
        /// <param name="fileData">The raw PDF file content.</param>
        /// <returns>The extracted document data.</returns>
        public Task<ExtractedDocumentDto> Execute(byte[] fileData)
        {
            return _scanner.ExtractPdfData(fileData);
        }
    }
}
