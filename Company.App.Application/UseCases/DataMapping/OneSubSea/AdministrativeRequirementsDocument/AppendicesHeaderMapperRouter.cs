using System;
using System.Collections.Generic;
using System.Text;
using Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements;
using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.AdministrativeRequirementsDocument
{
    public sealed class AppendicesHeaderMapperRouter : IAppendicesHeaderMapperRouter
    {
        private readonly IAppendicesHeaderMapper _appendicesHeaderMapper;
        
        public AppendicesHeaderMapperRouter(IAppendicesHeaderMapper appendicesHeaderMapper)
        {
            _appendicesHeaderMapper = appendicesHeaderMapper;
        }

        public IAppendicesHeaderMapper Resolve(DocumentType documentType)
        {
            return documentType switch
            {
                DocumentType.HSSERequirementsForSuppliers => _appendicesHeaderMapper,
                DocumentType.SupplierFinalInspectionSpecification => _appendicesHeaderMapper,
                DocumentType.SupplierPackingMarkingandShippingInstruction => _appendicesHeaderMapper,
                DocumentType.TraceabilitySpecificationForSuppliers => _appendicesHeaderMapper,
                DocumentType.SupplierDocumentationSpecification => _appendicesHeaderMapper,
                DocumentType.SpecialrequirementForPosreceivedFromBrazil => _appendicesHeaderMapper,
                _ => throw new NotSupportedException(
                    $"No appendices header mapper is registered for document type '{documentType}'.")
            };
        }
    }
}
