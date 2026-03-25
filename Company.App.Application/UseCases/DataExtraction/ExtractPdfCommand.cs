using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataExtraction.Models;
using MediatR;

namespace Company.App.Application.UseCases.DataExtraction
{
    public record ExtractPdfCommand(byte[] FileData) : IRequest<Result<ExtractedDocumentDto>>;
}
