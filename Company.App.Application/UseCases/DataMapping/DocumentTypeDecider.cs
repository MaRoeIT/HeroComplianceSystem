using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Specification;

namespace Company.App.Application.UseCases.DataMapping
{
    public class DocumentTypeDecider : IDocumentTypeDecider
    {
        private readonly IsOrderDocumentSpec _spec;

        public DocumentTypeDecider(IsOrderDocumentSpec spec)
        {
            _spec = spec;
        }

        public DocumentType Decide(DocumentHeaderContextDto context)
        {
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

            return DocumentType.Unknown;
        }

        private static string Normalize(string value)
        {
            return value.Trim();
        }
    }
}