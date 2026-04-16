using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces
{
    /// <summary>
    /// Defines a method that maps data from an extracted document to a collection of purchase order overhead entities.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for interpreting the provided document and
    /// extracting relevant overhead information. The mapping logic may vary depending on the document format or
    /// business rules.</remarks>
    public interface IPurchaseOrderOverheadMapper
    {
        PurchaseOrderOverhead Map(ExtractedDocumentDto document);
    }
}
