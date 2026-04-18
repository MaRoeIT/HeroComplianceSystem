using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements
{
    public interface IAdministrativeRequirementsHeaderMapper
    {
        AdministrativeRequirementsHeader Map(ExtractedDocumentDto document);
    }
}
