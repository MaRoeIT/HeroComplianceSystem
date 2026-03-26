using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.Interfaces
{
    public interface IDocumentTypeDecider
    {
        DocumentType Decide(DocumentHeaderContextDto context);
    }
}
