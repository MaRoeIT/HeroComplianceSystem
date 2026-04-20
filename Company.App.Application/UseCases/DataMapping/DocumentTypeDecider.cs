using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Specification;

namespace Company.App.Application.UseCases.DataMapping
{
    /// <summary>
    /// Determines the type of an extracted document by inspecting
    /// candidate header lines and evaluating them against domain specifications.
    /// </summary>
    public class DocumentTypeDecider : IDocumentTypeDecider
    {
        private readonly IsOrderDocumentSpec _spec;

        /// <summary>
        /// Initializes a new instance of the DocumentTypeDecider class using the specified order document
        /// specification.
        /// </summary>
        /// <param name="spec">The specification used to determine whether a document is considered an order document. Cannot be null.</param>
        public DocumentTypeDecider(IsOrderDocumentSpec spec)
        {
            _spec = spec;
        }

        /// <summary>
        /// Determines the type of document represented by the specified header context.
        /// </summary>
        /// <remarks>The method examines each candidate line in the provided context and returns the first
        /// matching document type based on predefined patterns. If none of the patterns match, DocumentType.Unknown is
        /// returned.</remarks>
        /// <param name="context">The document header context containing candidate lines to analyze for document type identification. Cannot
        /// be null.</param>
        /// <returns>A value of the DocumentType enumeration indicating the identified document type. Returns
        /// DocumentType.Unknown if no known document type is matched.</returns>
        public DocumentType Decide(DocumentHeaderContextDto context)
        {
            // Inspect candidate header lines in order to identify the document type.
            foreach (var line in context.CandidateLines)
            {
                var text = Normalize(line.Text);

                if (_spec.IsPurchaseOrder(text))
                    return DocumentType.PurchaseOrder;

                if (_spec.IsMaterialDocumentationPackage(text))
                    return DocumentType.MaterialDocumentationPackage;

                if (_spec.IsAdministrativeRequirements(text))
                    return DocumentType.AdministrativeRequirements;

                if (_spec.IsHSSERequirementsForSuppliers(text))
                    return DocumentType.HSSERequirementsForSuppliers;
                
                if (_spec.IsSupplierFinalInspectionSpecification(text))
                    return DocumentType.SupplierFinalInspectionSpecification;

                if (_spec.IsSupplierPackingMarkingandShippingInstruction(text))
                    return DocumentType.SupplierPackingMarkingandShippingInstruction;

                if (_spec.IsTraceabilitySpecificationForSuppliers(text))
                    return DocumentType.TraceabilitySpecificationForSuppliers;

                if (_spec.IsSupplierDocumentationSpecification(text))
                    return DocumentType.SupplierDocumentationSpecification;
            }

            // Return Unknown when no known document pattern is matched.
            return DocumentType.Unknown;
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the specified string.
        /// </summary>
        /// <param name="value">The string to normalize by trimming white-space characters from both ends. Cannot be null.</param>
        /// <returns>A new string that is equivalent to the input string but without leading or trailing white-space characters.
        /// If the input consists solely of white-space characters, returns an empty string.</returns>
        private static string Normalize(string value)
        {
            return value.Trim();
        }
    }
}