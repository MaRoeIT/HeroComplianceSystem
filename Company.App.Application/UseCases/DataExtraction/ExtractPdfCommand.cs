using Company.App.Application.Shared;
using MediatR;
using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.UseCases.DataExtraction
{
    public class ExtractPdfCommand : IRequest<Result<ExtractedDocumentDto>>
    {
        public Stream Stream { get; }

        public ExtractPdfCommand(Stream stream)
        {
            Stream = stream;
        }
    }
}
