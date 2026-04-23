using Company.App.Application.Interfaces;
using Company.App.Application.Interfaces.OneSubSea.MaterialDocumentationPackage;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.MaterialDocumentationPackageDocument
{
    public sealed class MapMaterialDocumentationPackageDocument : IDocumentMapper
    {
        public DocumentType SupportedType => DocumentType.MaterialDocumentationPackage;

        private readonly IMaterialDocumentationPackageHeaderMapper _materialDocumentationPackageHeaderMapper;
        private readonly IMaterialReportMapper _materialReportMapper;
        
        public MapMaterialDocumentationPackageDocument(IMaterialDocumentationPackageHeaderMapper headerMapper, IMaterialReportMapper materialReportMapper)
        {
            _materialDocumentationPackageHeaderMapper = headerMapper;
            _materialReportMapper = materialReportMapper;
        }

        public object Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;

            var firstPage = GetLinesOnPage(lines, 1);

            var orderNumber = GetValueByPattern(1,
                GetFirstLineContaining(firstPage, "Purchase Order:").Text, 8, 12);

            var orderDate = ParseDateFromLine(
                GetFirstLineContaining(firstPage, "Purchase order date:"));

            var header = _materialDocumentationPackageHeaderMapper.Map(document);

            var materialReports = _materialReportMapper.Map(document);

            return new MaterialDocumentationPackage(
                orderNumber,
                orderDate,
                header,
                materialReports);

        }
    }
}
