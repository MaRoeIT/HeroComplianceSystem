using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.Interfaces;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    public class DataMapperRouter : IDataMapperRouter
    {
        private readonly IDocumentTypeDecider _documentTypeDecider;
        private readonly IEnumerable<IDocumentMapper> _mappers;

        public DataMapperRouter(
            IDocumentTypeDecider documentTypeDecider, IEnumerable<IDocumentMapper> mappers)
        {
            _documentTypeDecider = documentTypeDecider;
            _mappers = mappers;
        }

        public DocumentMappingResultDto Route(ExtractedDocumentDto document)
        {
            var context = BuildHeaderContext(document);
            var documentType = _documentTypeDecider.Decide(context);

            var mapper = _mappers.FirstOrDefault(x => x.SupportedType == documentType);

            if (mapper is null)
            {
                return new DocumentMappingResultDto
                {
                    DocumentType = documentType,
                    ExtractedDocument = document,
                    PayLoad = null
                };
            }

            return new DocumentMappingResultDto
            {
                DocumentType = documentType,
                ExtractedDocument = document,
                PayLoad = mapper.Map(document)
            };
        }

        private static DocumentHeaderContextDto BuildHeaderContext(ExtractedDocumentDto document)
        {
            var candidatLines = document.Lines
                .Where(x => x.PageNumber == 1)
                .OrderByDescending(x => x.Y)
                .ToList();

            return new DocumentHeaderContextDto
            {
                CandidateLines = candidatLines
            };
        }
    }
}
