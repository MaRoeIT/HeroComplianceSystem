using Company.App.Application.Interfaces.OneSubSea.SharedMappers;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static Company.App.Application.UseCases.DataMapping.Services.RegexSearchService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.MaterialDocumentationPackageDocument
{
    public sealed class MapMdpBasicDataText : IBasicDataTextMapper
    {
        public DocumentType SupportedType => DocumentType.MaterialDocumentationPackage;

        public BasicDataText Map(List<ExtractedLineDto> contentLines)
        {
            var deliveryRequirementExpiryDate = GetValueByLabel(contentLines, "SEAL, ");

            var sealSQDescription = GetValueByLabel(contentLines, "SQ-seal");

            var sealEngineeringPartNumber = RemoveLabel(GetValueByLabel(contentLines, "Seal Engineering part number"), "Seal Engineering part number");

            var tSeal = RemoveLabel(GetValueByLabel(contentLines, "T-Seal:"), "T-Seal: ");

            var antiExtrusionRings = RemoveLabel(GetValueByLabel(contentLines, "Anti-extrution rings"), "Anti-extrution rings: ");

            return new BasicDataText(
                deliveryRequirementExpiryDate,
                sealSQDescription,
                sealEngineeringPartNumber,
                tSeal,
                antiExtrusionRings);
        }
    }
}
