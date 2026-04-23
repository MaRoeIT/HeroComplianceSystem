using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces.OneSubSea.PurchaseOrderDocument
{
    /// <summary>
    /// Defines a method that maps an extracted document to a collection of purchase order items.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for interpreting the data in an extracted
    /// document and producing a corresponding list of purchase order items. The mapping logic may vary depending on the
    /// document format or business rules.</remarks>
    public interface IItemMapper
    {
        IReadOnlyList<PurchaseOrderItem> Map(ExtractedDocumentDto document);
    }
}
