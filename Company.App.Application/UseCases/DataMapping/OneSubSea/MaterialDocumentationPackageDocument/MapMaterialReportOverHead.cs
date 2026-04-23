using Company.App.Application.Interfaces.OneSubSea.MaterialDocumentationPackage;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.MaterialDocumentationPackageDocument
{
    public sealed class MapMaterialReportOverHead : IMaterialReportOverHeadMapper
    {
        public MaterialReportOverHead Map(List<ExtractedLineDto> materialReportLines, List<ExtractedWordDto> words)
        {
            var lines = materialReportLines
                .Where(l => l.Y <= 650);

            var baseUnitOfMeasure = GetValueAfterLabel(lines, "Base Unit of Measure");
            var grossWeight = GetValueAfterLabel(lines, "Gross Weight");
            var netWeight = GetValueAfterLabel(lines, "Net Weight");
            var basicMaterial = GetValueAfterLabel(lines, "Basic Material");
            var commodityCode = GetValueAfterLabel(lines, "Commodity Code");
            var exportControlCode = GetValueAfterLabel(lines, "Export Control Code");
            var manufacturer = GetValueAfterLabel(lines, "Manufacturer");
            var manufacturerPartNumber = GetValueAfterLabel(lines, "Manuf.Part No.");
            var mirId = GetValueAfterLabel(lines, "MIR ID");
            var mirTitle = GetValueAfterLabel(lines, "MIR Title");
            var mirDescription = GetValueAfterLabel(lines, "MIR Desc.");
            var criticality = GetValueAfterLabel(lines, "Criticality");
            var traceabilityType = GetValueAfterLabel(lines, "Traceability type");
            var serialNumberProfile = GetValueAfterLabel(lines, "Serial No. Profile");
            var shelfLife = GetValueAfterLabel(lines, "Shelf Life");
            var qMControlKey = GetValueAfterLabel(lines, "QM Control Key");

            var inspectionSetupLines = GetLinesFromTargetLineToTargetLine(lines, "QM Control Key", "Classification");
            var inspectionSetups = (GetPartOfLinesRelativeToX(words, inspectionSetupLines, 165, 600))
                .Select(l => l.Text).ToList();
            
            var classificationLines = GetLinesFromTargetLineToTargetLine(lines, "Classification", "Other Related Docs", true)
                    .Where(l => !l.Text.Contains("Other Related Docs"));
            var classifications = (GetPartOfLinesRelativeToX(words, classificationLines, 165, 600))
                .Select(l => l.Text).ToList();

            var otherRelatedDocsLines = GetLinesFromTargetLineToTargetLine(lines, "Other Related Docs", "Inspection Requirements", true)
                    .Where(l => !l.Text.Contains("Inspection Requirements"));
            var otherRelatedDocs = (GetPartOfLinesRelativeToX(words, otherRelatedDocsLines, 165, 600))
                .Select(l => l.Text).ToList();

            var basicDataTextContainer = new List<ExtractedLineDto>();

            var basicDataTextLines = GetLinesFromTargetLineToTargetLine(lines, "Basic Data Text", "Characteristics", true)
                    .Where(l => !l.Text.Contains("Characteristics") && l.Y <= 650);
            var basicDataTextItems = (GetPartOfLinesRelativeToX(words, basicDataTextLines, 165, 600));

            foreach (var item in basicDataTextItems)
                basicDataTextContainer.Add(item);


            var basicDataTextMapper = new MapMdpBasicDataText();
            var basicDataText = basicDataTextMapper.Map(basicDataTextContainer);

            return new MaterialReportOverHead(
                baseUnitOfMeasure,
                grossWeight,
                netWeight,
                basicMaterial,
                commodityCode,
                exportControlCode,
                manufacturer,
                manufacturerPartNumber,
                mirId,
                mirTitle,
                mirDescription,
                criticality,
                traceabilityType,
                serialNumberProfile,
                shelfLife,
                qMControlKey,
                inspectionSetups,
                classifications,
                otherRelatedDocs,
                basicDataText
                );
        }
    }
}
