using Company.App.Application.Interfaces.OneSubSea.MaterialDocumentationPackage;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.MaterialDocumentationPackageDocument
{
    public sealed class MapMaterialReport : IMaterialReportMapper
    {
        private readonly IMaterialReportHeaderMapper _materialReportHeaderMapper;
        private readonly IMaterialReportOverHeadMapper _materialReportOverHeadMapper;
        private readonly IMaterialReportCharacteristicsMapper _materialReportCharacteristicsMapper;

        public MapMaterialReport (IMaterialReportHeaderMapper header, IMaterialReportOverHeadMapper overHead, IMaterialReportCharacteristicsMapper characteristics)
        {
            _materialReportHeaderMapper = header;
            _materialReportOverHeadMapper = overHead;
            _materialReportCharacteristicsMapper = characteristics;
        }

        public IReadOnlyList<MaterialReport> Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;
            var words = document.Words;

            var materialReportsLines = GroupMaterialReportPages(lines);

            var result = new List<MaterialReport>();
            
            foreach (var materialReportLines in materialReportsLines)
            {
                var header = _materialReportHeaderMapper.Map(materialReportLines);
                var overHead = _materialReportOverHeadMapper.Map(materialReportLines, words);
                var characteristics = _materialReportCharacteristicsMapper.Map(materialReportLines);

                var report = new MaterialReport(header, overHead, characteristics);

                result.Add(report);
            }

            return result;

        }
    }
}
