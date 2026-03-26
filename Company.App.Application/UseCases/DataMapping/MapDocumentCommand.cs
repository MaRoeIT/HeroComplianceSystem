using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataMapping.Models;
using MediatR;

namespace Company.App.Application.UseCases.DataMapping
{
    public record MapDocumentCommand(byte[] FileData) : IRequest<Result<DocumentMappingResultDto>>;
}
