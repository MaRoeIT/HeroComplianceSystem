using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;

namespace Company.App.Application.Interfaces
{
    public interface IBasicDataTextMapper
    {
        BasicDataText Map(List<ExtractedLineDto> contentLines);
    }
}
