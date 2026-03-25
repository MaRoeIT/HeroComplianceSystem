using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataExtraction
{
    public class ExtractPdfUseCase
    {
        private readonly IPdfDataExtractor _scanner;

        public ExtractPdfUseCase(IPdfDataExtractor scanner)
        {
            _scanner = scanner;
        }

        public Task<ExtractedDocumentDto> Execute(byte[] fileData)
        {
            return _scanner.ExtractPdfData(fileData);
        }
    }
}
