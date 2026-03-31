using Company.App.Application.UseCases.DataMapping.Models;

namespace Company.App.Application.Interfaces
{
    /// <summary>
    /// Defines logic for identifying the type of a document
    /// based on extracted header or contextual information.
    /// </summary>
    public interface IDocumentTypeDecider
    {
        /// <summary>
        /// Determines the document type from the supplied header context.
        /// </summary>
        /// <param name="context">Relevant extracted header lines from the document.</param>
        /// <returns>The detected document type.</returns>
        DocumentType Decide(DocumentHeaderContextDto context);
    }
}
