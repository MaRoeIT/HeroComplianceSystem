using Company.App.Application.Interfaces;
using Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using AdministrativeRequirementsEntity = Company.App.Domain.Entities.OneSubSea.AdministrativeRequirements;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea
{
    /// <summary>
    /// Maps extracted Administrative Requirements documents
    /// into a structured payload.
    /// </summary>
    public sealed class MapAdministrativeRequirementsDocument : IDocumentMapper
    {
        /// <summary>
        /// Gets the document type supported by this mapper.
        /// </summary>
        public DocumentType SupportedType => DocumentType.AdministrativeRequirements;

        private readonly IAdministrativeRequirementsHeaderMapper _administrativeRequirementsHeaderMapper;

        public MapAdministrativeRequirementsDocument(IAdministrativeRequirementsHeaderMapper administrativeRequirementsHeaderMapper)
        {
            _administrativeRequirementsHeaderMapper = administrativeRequirementsHeaderMapper;
        }

        /// <summary>
        /// Maps the specified extracted document to an object representing the mapped result.
        /// </summary>
        /// <remarks>This method currently returns a placeholder object. The mapping logic will be
        /// implemented in a future version.</remarks>
        /// <param name="document">The extracted document to be mapped. Cannot be null.</param>
        /// <returns>An object containing the result of the mapping operation.</returns>
        public object Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;

            var firstPage = GetLinesOnPage(lines, 1);

            var documentId = GetLinesFromTargetLine(firstPage, "Global SC Procedure", 1).Select(l => l.Text).FirstOrDefault();

            var issueDate = (DateOnly)ParseDateFromLines(GetLinesFromTargetLine(firstPage, "Ver. Status Issue date", 3));

            var header = _administrativeRequirementsHeaderMapper.Map(document);

            return new AdministrativeRequirementsEntity(
                documentId,
                issueDate,
                header);
        }
    }
}
