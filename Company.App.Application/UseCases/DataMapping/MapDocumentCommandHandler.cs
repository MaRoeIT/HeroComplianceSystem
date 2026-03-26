using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataMapping.Models;
using MediatR;

namespace Company.App.Application.UseCases.DataMapping
{
    public class MapDocumentCommandHandler : IRequestHandler<MapDocumentCommand, Result<DocumentMappingResultDto>>
    {
        private readonly IPdfDataExtractor _pdfDataExtractor;
        private readonly IDataMapperRouter _router;

        public MapDocumentCommandHandler(IPdfDataExtractor pdfDataExtractor, IDataMapperRouter router)
        {
            _pdfDataExtractor = pdfDataExtractor;
            _router = router;
        }

        public async Task<Result<DocumentMappingResultDto>> Handle(MapDocumentCommand request, CancellationToken cancellationToken)
        {
            var extractedDocument = await _pdfDataExtractor.ExtractPdfData(request.FileData);

            if (extractedDocument is null)
            {
                return new Result<DocumentMappingResultDto>(
                    null,
                    false,
                    "Failed to extract document data.");
            }

            var result =  _router.Route(extractedDocument);

            if (result.DocumentType == Models.DocumentType.Unknown)
            {
                return new Result<DocumentMappingResultDto>(
                    result,
                    false,
                    "Document type could not be identified.");
            }

            return new Result<DocumentMappingResultDto>(result, true);
        }
    }
}
