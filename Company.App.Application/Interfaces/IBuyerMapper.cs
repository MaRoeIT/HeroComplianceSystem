using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces
{
    /// <summary>
    /// Defines a method that maps an extracted document data transfer object to a buyer domain model.
    /// </summary>
    public interface IBuyerMapper
    {
        Buyer Map(ExtractedDocumentDto document);
    }
}
