using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static Company.App.Application.UseCases.DataMapping.Helper.IsPriceFormatValues;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    public sealed class MapItems : IItemMapper
    {
        public PurchaseOrderItem Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var words = document.Words;

            var itemLines = lines
                .Where(l =>
                HasPriceFormatValues(l.Text, 2) &&
                GetWordsFromLine(words, l).Count() == 8 &&
                !l.Text.Contains("Your ref"))
                .ToList();

            foreach (var l in itemLines)
            {
                var itemContent = GetLinesFromTargetLine(itemLines, l.Text, 8, true).ToList();
            }
        }
    }
}
