using Company.App.Application.Interfaces.OneSubSea.MaterialDocumentationPackage;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Domain.Entities.OneSubSea;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.MaterialDocumentationPackageDocument
{
    public sealed class MapMaterialDocumentationPackageHeader : IMaterialDocumentationPackageHeaderMapper
    {
        public MaterialDocumentationPackageHeader Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;

            var dataSection = GetLinesFromTargetLineToTargetLine(GetLinesOnPage(lines, 1), "Purchase order date", "Email", true);

            var purchaseOrderDate = ParseDateFromLine(GetFirstLineContaining(dataSection, "Purchase order date"));

            var mdpIssueDateTime = ParseDateFromLine(GetFirstLineContaining(dataSection, "MDP Issue date"));

            var documentRevision = GetValueAfterLabel(dataSection, "Document revision");

            var ourReference = GetValueAfterLabel(dataSection, "Our Reference");

            var buyer = RemoveSymbolsFromString(RemoveNumbersFromString(GetValueAfterLabel(dataSection, "Buyer / Telephone number")));

            var telephoneNumber = RemoveSymbolsFromString(RemoveCharFromString(GetValueAfterLabel(dataSection, "Buyer / Telephone number")), "+");

            var email = GetValueAfterLabel(dataSection, "Email");

            return new MaterialDocumentationPackageHeader(
                purchaseOrderDate,
                mdpIssueDateTime,
                documentRevision,
                ourReference,
                buyer,
                telephoneNumber,
                email);

        }
    }
}
