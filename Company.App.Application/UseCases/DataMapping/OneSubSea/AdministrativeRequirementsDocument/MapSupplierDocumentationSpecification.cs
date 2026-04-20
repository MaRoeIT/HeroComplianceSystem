using Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.AdministrativeRequirementsDocument
{
    public sealed class MapSupplierDocumentationSpecification : MapAppendicesDocumentBase
    {
        public MapSupplierDocumentationSpecification(IAppendicesHeaderMapperRouter router)
            : base(router) { }

        public override DocumentType SupportedType => DocumentType.SupplierDocumentationSpecification;
    }
}