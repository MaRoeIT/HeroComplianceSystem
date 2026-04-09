using System.Text.RegularExpressions;
using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static Company.App.Application.UseCases.DataMapping.Services.RegexSearchService;
using static Company.App.Application.UseCases.DataMapping.Helper.IsPriceFormatValues;
using static Company.App.Application.UseCases.DataMapping.Helper.IsItemLine;


namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    public sealed class MapMaterialDescription : IMaterialDescriptionMapper
    {
        public MaterialDescription Map(ExtractedDocumentDto document, ExtractedLineDto itemLine, string materialNumber)
        {
            var words = document.Words;
            var lines = document.Lines;

            var itemLines = GetItemLines(lines, words);
            
            var itemBlock = GetItemContent(lines, itemLines)
                .FirstOrDefault(b => SameLine(b.ItemLine, itemLine));

            var contentLines = itemBlock?.ContentLines?.ToList()
                ?? new List<ExtractedLineDto>();


            var materialName = GetValueByLabel(contentLines, "SEAL,");
            
            var traceabilityCode = GetValueByLabel(contentLines, "Traceability Code:");

            var docRefTraceabilitySpecificationForSuppliers = RemoveLabel(RemoveLabel(GetValueByLabel(contentLines, "Doc.ref."), "Doc.ref."), " (Traceability Specification for Suppliers)");

            var outlineAgreementNumber = RemoveLabel(GetValueByLabel(contentLines, "Outline Agreement No:"), "Outline Agreement No: ");

            var manufacturer = RemoveLabel(GetValueByLabel(contentLines, "Manufacturer:"), "Manufacturer: ");

            var workBreakdownStructure = RemoveLabel(GetValueByLabel(contentLines, "WBS:"), "WBS: ");

            var manufacturerPartNo = RemoveLabel(GetValueByLabel(contentLines, "Mfr."), "Mfr. Part Number: ");

            var deliveryDate = GetValueByLabel(contentLines, "Delivery Date:");

            deliveryDate = string.IsNullOrWhiteSpace(deliveryDate)
                ? string.Empty
                : deliveryDate
                    .Replace("Delivery Date:", "")
                    .Replace(ContextTrimParse(deliveryDate, @" Quantity\:\s*\d\.\d{1,3}"), "")
                    .Trim();

            var basicDataTextMapper = new MapBasicDataText();
            var basicDataText = basicDataTextMapper.Map(contentLines);

            return new MaterialDescription(
                materialNumber,
                materialName,
                traceabilityCode,
                docRefTraceabilitySpecificationForSuppliers,
                outlineAgreementNumber,
                basicDataText,
                manufacturer,
                workBreakdownStructure,
                manufacturerPartNo,
                deliveryDate);
        }


        
        
    }
}
