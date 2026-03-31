using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataExtraction.Models;
using MediatR;

namespace Company.App.Application.UseCases.DataExtraction
{
    /// <summary>
    /// Handles PDF extraction requests by delegating to the PDF data extractor
    /// and wrapping the result in a standard application result object.
    /// </summary>
    public class ExtractPdfCommandHandler : IRequestHandler<ExtractPdfCommand, Result<ExtractedDocumentDto>>
    {
        private readonly IPdfDataExtractor _scanner;

        public ExtractPdfCommandHandler(IPdfDataExtractor scanner)
        {
            _scanner = scanner;
        }

        public async Task<Result<ExtractedDocumentDto>> Handle(ExtractPdfCommand request, CancellationToken cancellationToken)
        {
            // Extract raw words and lines from the uploaded PDF.
            var document = await _scanner.ExtractPdfData(request.FileData);

            // Return a failed result if extraction produced no document.
            if (document == null)
            {
                return new Result<ExtractedDocumentDto>(null, false, "Failed to extract PDF file.");
            }

            // Wrap the extracted document in a successful application result.
            return new Result<ExtractedDocumentDto>(document, true);
        }
    }
}
