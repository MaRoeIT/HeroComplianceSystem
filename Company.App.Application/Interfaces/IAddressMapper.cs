using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces
{
    public interface IAddressMapper<out TAddress>
        where TAddress : Address
    {
        TAddress Map(ExtractedDocumentDto document);
    }
}
