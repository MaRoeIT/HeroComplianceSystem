using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces
{
    public interface ISellerMapper
    {
        Seller Map(ExtractedDocumentDto document);
    }
}
