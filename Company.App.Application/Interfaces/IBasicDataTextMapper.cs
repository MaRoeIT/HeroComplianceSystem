using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;

namespace Company.App.Application.Interfaces
{
    /// <summary>
    /// Defines a contract for mapping a collection of extracted text lines to a structured data representation.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for interpreting and converting raw text
    /// line data into a strongly typed format. This is typically used in scenarios where text extraction from documents
    /// or files needs to be transformed into domain-specific objects.</remarks>
    public interface IBasicDataTextMapper
    {
        BasicDataText Map(List<ExtractedLineDto> contentLines);
    }
}
