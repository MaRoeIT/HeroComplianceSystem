using Company.App.Application.Interfaces.OneSubSea.MaterialDocumentationPackage;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.MaterialDocumentationPackageDocument
{
    public sealed class MapMaterialReportCharacteristics : IMaterialReportCharacteristicsMapper
    {
        public MaterialReportCharacteristics Map(List<ExtractedLineDto> materialReportLines)
        {
            var lines = materialReportLines
                .Where(l => l.Y <= 650);

            var characteristicsItems = GetLinesFromTargetLine(lines, "Characteristics:");

            var consequenceOfFailure = GetValueAfterLabel(characteristicsItems, "Consequence of Failure");
            var createdBy = GetValueAfterLabel(characteristicsItems, "Created By");
            var criticalRating = GetValueAfterLabel(characteristicsItems, "Critical Rating");
            var dataAdditional = GetValueAfterLabel(characteristicsItems, "Data - Additional");
            var division = GetValueAfterLabel(characteristicsItems, "Division");
            var internalDiameter = GetValueAfterLabel(characteristicsItems, "Internal Diameter");
            var labDesignOffice = GetValueAfterLabel(characteristicsItems, "Lab / Design Office");
            var length = GetValueAfterLabel(characteristicsItems, "Length");
            var mirid = GetValueAfterLabel(characteristicsItems, "MIR ID");
            var manufacturer = GetValueAfterLabel(characteristicsItems, "Manufacturer");
            var manufacturerPartNumber = GetValueAfterLabel(characteristicsItems, "Manufacturer Part No");
            var material = GetValueAfterLabel(characteristicsItems, "Material");
            var materialGroup = GetValueAfterLabel(characteristicsItems, "Material Group");
            var matlBasic = GetValueAfterLabel(characteristicsItems, "Matl - Basic");
            var outsideDiameter = GetValueAfterLabel(characteristicsItems, "Outside Diameter");
            var polymerSealType = GetValueAfterLabel(characteristicsItems, "Polymer Seal Type");
            var probabilityOfFailure = GetValueAfterLabel(characteristicsItems, "Probability of Failure");
            var sdrLandQM = GetValueAfterLabel(characteristicsItems, "SDRL & QM");
            var standardMaterial = GetValueAfterLabel(characteristicsItems, "Standard Material");
            var traceabilityCode = GetValueAfterLabel(characteristicsItems, "Traceability Code");
            var xPlantStatus = GetValueAfterLabel(characteristicsItems, "X Plant Status");

            return new MaterialReportCharacteristics(
                consequenceOfFailure,
                createdBy,
                criticalRating,
                dataAdditional,
                division,
                internalDiameter,
                labDesignOffice,
                length,
                mirid,
                manufacturer,
                manufacturerPartNumber,
                material,
                materialGroup,
                matlBasic,
                outsideDiameter,
                polymerSealType,
                probabilityOfFailure,
                sdrLandQM,
                standardMaterial,
                traceabilityCode,
                xPlantStatus);
        }
    }
}
