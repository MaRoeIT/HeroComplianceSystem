using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataExtraction.Models;
using MediatR;

namespace Company.App.Application.UseCases.DataExtraction
{
    /// <summary>
    /// Command used to request extraction of textual PDF data
    /// from an uploaded file.
    /// </summary>
    /// <param name="FileData">The raw PDF file content.</param>
    public record ExtractPdfCommand(byte[] FileData) : IRequest<Result<ExtractedDocumentDto>>;
}
