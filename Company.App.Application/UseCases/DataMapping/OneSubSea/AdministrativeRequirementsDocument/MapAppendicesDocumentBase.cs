using Company.App.Application.Interfaces;
using Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.AdministrativeRequirementsDocument
{
    public abstract class MapAppendicesDocumentBase : IDocumentMapper
    {
        private readonly IAppendicesHeaderMapperRouter _router;

        protected MapAppendicesDocumentBase(IAppendicesHeaderMapperRouter router)
        {
            _router = router;
        }

        public abstract DocumentType SupportedType { get; }

        public object Map(ExtractedDocumentDto document)
        {
            var mapper = _router.Resolve(SupportedType);
            return mapper.Map(document);
        }
    }
}