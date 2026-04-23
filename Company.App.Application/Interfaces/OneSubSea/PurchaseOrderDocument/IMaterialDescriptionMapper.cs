using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;

namespace Company.App.Application.Interfaces.OneSubSea.PurchaseOrderDocument
{
    /// <summary>
    /// Defines a method that maps extracted document and line data to a material description object.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for transforming data from document and
    /// line DTOs into a domain-specific material description. This interface is typically used in scenarios where
    /// material information must be derived from parsed or extracted document content.</remarks>
    public interface IMaterialDescriptionMapper
    {
        MaterialDescription Map(ExtractedDocumentDto document, ExtractedLineDto itemLine, string materialNumber);
    }
}
