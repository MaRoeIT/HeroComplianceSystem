using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataMapping.Models;
using MediatR;

namespace Company.App.Application.UseCases.DataMapping
{
    /// <summary>
    /// Handles document mapping requests by first extracting PDF data
    /// and then routing the extracted document to the correct mapper.
    /// </summary>
    public class MapDocumentCommandHandler : IRequestHandler<MapDocumentCommand, Result<DocumentMappingResultDto>>
    {
        private readonly IPdfDataExtractor _pdfDataExtractor;
        private readonly IDataMapperRouter _router;

        public MapDocumentCommandHandler(IPdfDataExtractor pdfDataExtractor, IDataMapperRouter router)
        {
            _pdfDataExtractor = pdfDataExtractor;
            _router = router;
        }

        /// <summary>
        /// Processes a document mapping command by extracting data from a PDF file and routing it to the appropriate
        /// document mapper.
        /// </summary>
        /// <remarks>Returns a failed result if the PDF data cannot be extracted or if the document type
        /// cannot be identified.</remarks>
        /// <param name="request">The command containing the PDF file data to be mapped.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A result containing the mapped document data if extraction and mapping succeed; otherwise, a failed result
        /// with an error message.</returns>
        public async Task<Result<DocumentMappingResultDto>> Handle(MapDocumentCommand request, CancellationToken cancellationToken)
        {
            // First extract raw textual and positional content from the PDF.
            var extractedDocument = await _pdfDataExtractor.ExtractPdfData(request.FileData);

            if (extractedDocument is null)
            {
                return new Result<DocumentMappingResultDto>(
                    null,
                    false,
                    "Failed to extract document data.");
            }

            // Route the extracted document to the correct mapper based on detected type.
            var result =  _router.Route(extractedDocument);

            // Return a failed result if the document type could not be identified.
            if (result.DocumentType == Models.DocumentType.Unknown)
            {
                return new Result<DocumentMappingResultDto>(
                    result,
                    false,
                    "Document type could not be identified.");
            }

            // Return the mapped payload when routing and type detection succeeded.
            return new Result<DocumentMappingResultDto>(result, true);
        }
    }
}
