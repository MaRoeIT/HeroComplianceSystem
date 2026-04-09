using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;

namespace Company.App.Application.Interfaces
{
    public interface IMaterialDescriptionMapper
    {
        MaterialDescription Map(ExtractedDocumentDto document, ExtractedLineDto itemLine, string materialNumber);
    }
}
