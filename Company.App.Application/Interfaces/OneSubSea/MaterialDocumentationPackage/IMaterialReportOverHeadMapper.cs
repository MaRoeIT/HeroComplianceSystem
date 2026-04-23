using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces.OneSubSea.MaterialDocumentationPackage
{
    public interface IMaterialReportOverHeadMapper
    {
        public MaterialReportOverHead Map(List<ExtractedLineDto> materialReportLines, List<ExtractedWordDto> words);
    }
}
