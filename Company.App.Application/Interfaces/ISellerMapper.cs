using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces
{
    /// <summary>
    /// Defines a contract for mapping an extracted document data transfer object to a seller domain model.
    /// </summary>
    public interface ISellerMapper
    {
        Seller Map(ExtractedDocumentDto document);
    }
}
