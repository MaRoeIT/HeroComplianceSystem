using MediatR;
using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataExtraction
{
    public class ExtractPdfCommandHandler
    : IRequestHandler<ExtractPdfCommand, Result<ExtractedDocumentDto>>
    {
        private readonly ExtractPdfUseCase _useCase;

        public ExtractPdfCommandHandler(ExtractPdfUseCase useCase)
        {
            _useCase = useCase;
        }

        public Task<Result<ExtractedDocumentDto>> Handle(
            ExtractPdfCommand request,
            CancellationToken cancellationToken)
        {
            var result = _useCase.Execute(request.Stream);

            return Task.FromResult(Result<ExtractedDocumentDto>.Success(result));
        }
    }
}
