using Company.App.Application.Interfaces.OneSubSea.PurchaseOrderDocument;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static Company.App.Application.UseCases.DataMapping.Helper.IsPriceFormatValues;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    /// <summary>
    /// Provides functionality to map extracted document data into a collection of purchase order items.
    /// </summary>
    /// <remarks>This class is typically used to transform structured document data, such as that obtained
    /// from OCR or data extraction processes, into domain-specific purchase order item objects. It relies on an
    /// external material description mapper to enrich item data. Instances of this class are immutable and
    /// thread-safe.</remarks>
    public sealed class MapItems : IItemMapper
    {
        private readonly IMaterialDescriptionMapper _materialDescriptionMapper;

        /// <summary>
        /// Initializes a new instance of the MapItems class with the specified material description mapper.
        /// </summary>
        /// <param name="materialDescriptionMapper">The mapper used to convert material descriptions to their corresponding representations. Cannot be null.</param>
        public MapItems(IMaterialDescriptionMapper materialDescriptionMapper)
        {
            _materialDescriptionMapper = materialDescriptionMapper;
        }

        /// <summary>
        /// Extracts and maps purchase order items from the specified document.
        /// </summary>
        /// <remarks>Only lines that contain exactly eight words are considered valid item lines and
        /// included in the result. The mapping relies on the structure of the document's lines and words.</remarks>
        /// <param name="document">The document containing extracted lines and words to be mapped into purchase order items. Cannot be null.</param>
        /// <returns>A read-only list of purchase order items mapped from the document. The list is empty if no valid items are
        /// found.</returns>
        public IReadOnlyList<PurchaseOrderItem> Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var words = document.Words;
           
            var itemLines = GetItemLines(lines, words);

            var result = new List<PurchaseOrderItem>();

            foreach (var itemLine in itemLines)
            {
                var lineWords = GetWordsFromLine(words, itemLine)
                    .OrderBy(w => w.X)
                    .ToList();

                if (lineWords.Count != 8)
                    continue;

                var itemNumber = lineWords[0].Text;
                var materialNumber = lineWords[1].Text;
                var revision = lineWords[2].Text;
                var quantity = lineWords[3].Text;
                var order = lineWords[4].Text;
                var unit = lineWords[5].Text;
                var netPrice = lineWords[6].Text;
                var netAmount = lineWords[7].Text;

                var materialDescription = _materialDescriptionMapper.Map(document, itemLine, materialNumber);

                var item = new PurchaseOrderItem(
                    itemNumber: itemNumber,
                    pageNumber: new HashSet<int> { itemLine.PageNumber},
                    materialDescriptions: materialDescription,
                    revision: revision,
                    quantity : quantity,
                    order : order,
                    unit : unit,
                    netPrice : netPrice,
                    netAmount : netAmount
                    );

                result.Add(item);
            }

            return result;
        }
    }
}
