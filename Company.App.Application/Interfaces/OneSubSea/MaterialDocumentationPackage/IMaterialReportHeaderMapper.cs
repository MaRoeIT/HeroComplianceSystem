using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces.OneSubSea.MaterialDocumentationPackage
{
    public interface IMaterialReportHeaderMapper
    {
        public MaterialReportHeader Map(List<ExtractedLineDto> materialReport);
    }
}
