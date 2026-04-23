using Company.App.Application.Interfaces.OneSubSea.PurchaseOrderDocument;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping.Helper;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Application.UseCases.DataMapping.Services;
using Company.App.Domain.Entities.OneSubSea;
using System.Diagnostics.Metrics;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;
using static System.Net.WebRequestMethods;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument
{
    /// <summary>
    /// Provides functionality to map extracted document data into a hierarchical collection of purchase order overhead
    /// sections.
    /// </summary>
    /// <remarks>This class parses structured sections from an extracted document and organizes them into a
    /// tree of purchase order overheads. It is intended for use when processing documents that follow a specific
    /// sectioned format, such as purchase orders with standardized overhead sections. Instances of this class are
    /// thread-safe for concurrent use.</remarks>
    public sealed class MapPurchaseOrderOverhead : IPurchaseOrderOverheadMapper
    {
        /// <summary>
        /// Maps the specified extracted document to a collection of purchase order overhead sections.
        /// </summary>
        /// <remarks>This method identifies and extracts specific overhead sections from the provided
        /// document based on predefined section headers. The returned list preserves the order of the sections as they
        /// appear in the document.</remarks>
        /// <param name="document">The extracted document data to be analyzed and mapped. Cannot be null.</param>
        /// <returns>A read-only list of purchase order overhead sections found in the document. The list will be empty if no
        /// relevant sections are found.</returns>
        public PurchaseOrderOverhead Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;

            var targets = new[] {
            "1. PURCHASE ORDER REVISIONS",
            "2. PURCHASE ORDER DOCUMENTS",
            "3. SCOPE OF WORK",
            "4. SPECIAL MARKING & PACKING REQUIREMENTS",
            "5. SPECIAL CONDITIONS OF PURCHASE",
            "6. SPECIAL ADMINISTRATIVE REQUIREMENTS",
            "7. CONTACT DETAILS FOR DOCUMENTATION AND COMMUNICATION"
        };

            var frontPage = GetLinesOnPage(lines, 1);
            var summaryLines = GetLinesFromTargetLine(frontPage, "Rev Quantity Order Unit", 0, true);


            var overheadLines = GetPagesByLineContent(lines, targets)
                .Where(l =>
                    l.Y <= 590 && l.Y >= 70)
                .ToList();

            var collective = summaryLines.Concat(overheadLines).OrderBy(l => l.PageNumber).ThenByDescending(l => l.Y).ToList();

            var summary = GetLinesFromTargetLineToTargetLine(collective, "Rev Quantity Order Unit", "Rev Quantity Order Unit", false);

            var overhead = GetPagesByLineContent(collective, targets)
                .Where(l =>
                    l.Y <= 575 && l.Y >= 70)
                .ToList();

            // List to hold all created subsection objects.
            var subSectionList = new List<PurchaseOrderOverhead>();

            // Summary object
            var summaryObject = new PurchaseOrderOverhead(
                pageNumbers: new HashSet<int>(summaryLines.Select(p => p.PageNumber)),
                sectionNumber: "",
                title: "Summary",
                content: summaryLines.Select(l => l.Text).ToList(),
                subSections: subSectionList
                );

            foreach (var target in targets)
            {
                var sectionLines = GetLinesFromTargetLineToTargetLine(
                    overhead,
                    target,
                    GetNextTarget(targets, target),
                    true).SkipLast(1).ToList();

                var subSection = new PurchaseOrderOverhead(
                    pageNumbers: new HashSet<int>(sectionLines.Select(p => p.PageNumber)),
                    sectionNumber: RemoveSymbolsFromString(RemoveCharFromString(sectionLines.Select(l => l.Text).FirstOrDefault())),
                    title: RemoveSymbolsFromString(RemoveNumbersFromString(sectionLines.Select(l => l.Text).FirstOrDefault())),
                    content: sectionLines.Select(l => l.Text).Skip(1).ToList(),
                    subSections: Array.Empty<PurchaseOrderOverhead>()
                    );

                subSectionList.Add(subSection);
            }

            return summaryObject;
        }

        private static string? GetNextTarget(string[] targets, string currentTarget)
        {
            var index = Array.IndexOf(targets, currentTarget);

            if (index < 0 || index == targets.Length - 1)
                return null;

            return targets[index + 1];
        }
    }
}
