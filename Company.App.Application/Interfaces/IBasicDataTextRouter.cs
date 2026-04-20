using Company.App.Application.Interfaces.OneSubSea.SharedMappers;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;

namespace Company.App.Application.Interfaces
{
    public interface IBasicDataTextRouter
    {
        IBasicDataTextMapper Resolve(DocumentType documentType);
    }
}
