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

        public DocumentTypeDecider(IsOrderDocumentSpec spec)
        {
            _spec = spec;
        }

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
            }

            // Return Unknown when no known document pattern is matched.
            return DocumentType.Unknown;
        }

        /// <summary>
        /// Applies basic normalization before text is evaluated against specifications.
        /// </summary>
        private static string Normalize(string value)
        {
            return value.Trim();
        }
    }
}