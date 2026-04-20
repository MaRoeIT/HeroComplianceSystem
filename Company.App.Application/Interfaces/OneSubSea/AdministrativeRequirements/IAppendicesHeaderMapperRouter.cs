using Company.App.Application.UseCases.DataMapping.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements
{
    public interface IAppendicesHeaderMapperRouter
    {
        IAppendicesHeaderMapper Resolve(DocumentType documentType);
    }
}
