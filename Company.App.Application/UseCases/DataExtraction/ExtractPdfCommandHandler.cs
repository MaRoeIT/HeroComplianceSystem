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

        /// <summary>
        /// Initializes a new instance of the ExtractPdfCommandHandler class with the specified PDF data extractor.
        /// </summary>
        /// <param name="scanner">The PDF data extractor used to process and extract information from PDF documents. Cannot be null.</param>
        public ExtractPdfCommandHandler(IPdfDataExtractor scanner)
        {
            _scanner = scanner;
        }

        /// <summary>
        /// Processes an ExtractPdfCommand to extract data from a PDF file and returns the extracted document
        /// information.
        /// </summary>
        /// <param name="request">The command containing the PDF file data to be processed for extraction.</param>
        /// <param name="cancellationToken">A token that can be used to request cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a Result object with the
        /// extracted document data if successful; otherwise, a failed Result with an error message.</returns>
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
