using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces.OneSubSea.PurchaseOrderDocument
{
    /// <summary>
    /// Defines a contract for mapping an extracted document data transfer object to a purchase order header domain
    /// model.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for converting data from the extracted
    /// document format into a strongly-typed purchase order header. This is typically used in scenarios where document
    /// parsing and domain model population are separated for maintainability and testability.</remarks>
    public interface IPurchaseOrderHeaderMapper
    {
        PurchaseOrderHeader Map(ExtractedDocumentDto document);
    }
}
