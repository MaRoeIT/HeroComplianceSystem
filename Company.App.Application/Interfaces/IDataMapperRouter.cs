using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.Interfaces
{
    public interface IDataMapperRouter
    {
        DocumentMappingResultDto Route(ExtractedDocumentDto documnet);
    }
}
