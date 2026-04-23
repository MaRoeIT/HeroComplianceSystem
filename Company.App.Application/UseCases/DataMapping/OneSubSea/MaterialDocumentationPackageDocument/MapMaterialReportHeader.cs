using Company.App.Application.Interfaces.OneSubSea.MaterialDocumentationPackage;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.MaterialDocumentationPackageDocument
{
    public sealed class MapMaterialReportHeader : IMaterialReportHeaderMapper
    {
        public MaterialReportHeader Map(List<ExtractedLineDto> materialReportLines)
        {
            var lines = materialReportLines
                .Where(l => l.Y >= 650);

            var pageNumbers = lines.Select(l => l.PageNumber).Distinct().ToHashSet();

            var reportDate = ParseDateFromLines(lines);

            var materialNumber = GetValueAfterLabel(lines, "Material");

            var description = GetValueAfterLabel(lines, "Description");

            var revisionLevel = GetValueAfterLabel(lines, "Revision Level");

            var plant = GetValueAfterLabel(lines, "Plant");

            return new MaterialReportHeader(
                pageNumbers,
                reportDate,
                materialNumber,
                description,
                revisionLevel,
                plant);
        }
    }
}
