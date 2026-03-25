using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataExtraction.Models;
using MediatR;

namespace Company.App.Application.UseCases.DataExtraction
{
    public class ExtractPdfCommandHandler : IRequestHandler<ExtractPdfCommand, Result<ExtractedDocumentDto>>
    {
        private readonly IPdfDataExtractor _scanner;

        public ExtractPdfCommandHandler(IPdfDataExtractor scanner)
        {
            _scanner = scanner;
        }

        public async Task<Result<ExtractedDocumentDto>> Handle(ExtractPdfCommand request, CancellationToken cancellationToken)
        {
            var document = await _scanner.ExtractPdfData(request.FileData);

            if (document == null)
            {
                return new Result<ExtractedDocumentDto>(null, false, "Failed to extract PDF file.");
            }

            return await Task.FromResult(
                new Result<ExtractedDocumentDto>(document, true));
        }
    }
}
