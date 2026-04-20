using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    /// <summary>
    /// Represents the supported document types that can be identified
    /// and mapped by the application.
    /// </summary>
    public enum DocumentType
    {
        // The document type could not be identified.
        Unknown = 0,

        // A purchase order document.
        PurchaseOrder = 1,

        // A material documentation package document.
        MaterialDocumentationPackage = 2,

        // An administrative requirements document.
        AdministrativeRequirements = 3,

        HSSERequirementsForSuppliers = 4,

        SupplierFinalInspectionSpecification = 5,

        SupplierPackingMarkingandShippingInstruction = 6,

        TraceabilitySpecificationForSuppliers = 7,

        SupplierDocumentationSpecification = 8,

        SpecialrequirementForPosreceivedFromBrazil = 9
    }
}
