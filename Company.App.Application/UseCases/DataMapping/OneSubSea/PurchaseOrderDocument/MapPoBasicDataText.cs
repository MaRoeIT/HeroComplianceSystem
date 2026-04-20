using Company.App.Application.Interfaces.OneSubSea.SharedMappers;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static Company.App.Application.UseCases.DataMapping.Services.RegexSearchService;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    /// <summary>
    /// Provides functionality to map a collection of extracted text lines to a structured basic data text
    /// representation based on predefined labels.
    /// </summary>
    /// <remarks>This class is intended for use in scenarios where text data, typically extracted from
    /// documents, needs to be parsed and mapped to strongly-typed data fields. The mapping is performed according to
    /// specific label conventions expected in the input data. This type is sealed and cannot be inherited.</remarks>
    public sealed class MapPoBasicDataText : IBasicDataTextMapper
    {

        /// <summary>
        /// Maps a collection of extracted line data to a new instance of the BasicDataText class by extracting and
        /// transforming relevant fields.
        /// </summary>
        /// <remarks>The mapping relies on specific label names within the content lines. If a required
        /// label is missing, the corresponding value in the result may be null or empty.</remarks>
        /// <param name="contentLines">The list of extracted line data to be mapped. Each item represents a labeled line of text from the source
        /// content. Cannot be null.</param>
        /// <returns>A BasicDataText object containing the mapped and processed values from the provided content lines.</returns>
        public DocumentType SupportedType => DocumentType.PurchaseOrder;

        public BasicDataText Map(List<ExtractedLineDto> contentLines)
        {
            var deliveryRequirementExpiryDate = GetValueByLabel(contentLines, "SQ-I");

            var sealSQDescription = GetValueByLabel(contentLines, "SQ-seal");

            var sealEngineeringPartNumber = RemoveLabel(GetValueByLabel(contentLines, "Seal Engineering part number"), "Seal Engineering part number");

            var tSeal = RemoveLabel(GetValueByLabel(contentLines, "T-Seal:"), "T-Seal: ");

            var antiExtrusionRings = RemoveLabel(GetValueByLabel(contentLines, "Anti-extrution rings"), "Anti-extrution rings: ");

            return new BasicDataText(
                deliveryRequirementExpiryDate,
                sealSQDescription,
                sealEngineeringPartNumber,
                tSeal,
                antiExtrusionRings);
        }
    }
}
