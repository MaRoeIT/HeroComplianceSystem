using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataExtraction
{
    public class ExtractPdfUseCase
    {
        private readonly IPdfDataExtractor _extractor;

        public ExtractPdfUseCase(IPdfDataExtractor extractor)
        {
            _extractor = extractor;
        }

        public ExtractedDocumentDto Execute(Stream stream)
        {
            return _extractor.Extract(stream);
        }
    }
}
