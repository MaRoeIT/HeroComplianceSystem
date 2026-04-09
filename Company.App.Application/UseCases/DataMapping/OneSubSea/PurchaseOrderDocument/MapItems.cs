using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static Company.App.Application.UseCases.DataMapping.Helper.IsPriceFormatValues;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    public sealed class MapItems : IItemMapper
    {
        private readonly IMaterialDescriptionMapper _materialDescriptionMapper;

        public MapItems(IMaterialDescriptionMapper materialDescriptionMapper)
        {
            _materialDescriptionMapper = materialDescriptionMapper;
        }

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
