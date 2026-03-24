using System.IO;
using Company.App.Application.UseCases.DataExtraction.Models;

namespace Company.App.Application.Interfaces
{
    public interface IPdfDataExtractor
    {
        ExtractedDocumentDto Extract(Stream stream);
    }
}
