using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;

namespace Company.App.Application.Interfaces.OneSubSea.SharedMappers
{
    public interface IBasicDataTextMapper
    {
        DocumentType SupportedType { get; }

        BasicDataText Map(List<ExtractedLineDto> contentLines);
    }
}