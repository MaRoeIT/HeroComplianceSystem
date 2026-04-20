using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;


namespace Company.App.Application.Interfaces
{
    internal interface ICharacteristicsItemMapper
    {
        MaterialReportCharacteristics Map(ExtractedDocumentDto document);
    }
}
