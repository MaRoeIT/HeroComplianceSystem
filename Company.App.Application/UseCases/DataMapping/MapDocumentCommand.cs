using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataMapping.Models;
using MediatR;

namespace Company.App.Application.UseCases.DataMapping
{
    /// <summary>
    /// Command used to request extraction and mapping of a PDF document
    /// into a typed business payload.
    /// </summary>
    /// <param name="FileData">The raw PDF file content.</param>
    public record MapDocumentCommand(byte[] FileData) : IRequest<Result<DocumentMappingResultDto>>;
}
