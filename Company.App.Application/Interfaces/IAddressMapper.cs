using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces
{
    /// <summary>
    /// Defines a contract for mapping an extracted document to an address of a specified type.
    /// </summary>
    /// <remarks>Implementations of this interface provide logic to convert or extract address information
    /// from a document data transfer object. This is typically used in scenarios where address extraction or
    /// transformation is required from structured document data.</remarks>
    /// <typeparam name="TAddress">The type of address to which the document is mapped. Must derive from Address.</typeparam>
    public interface IAddressMapper<out TAddress>
        where TAddress : Address
    {
        TAddress Map(ExtractedDocumentDto document);
    }
}
