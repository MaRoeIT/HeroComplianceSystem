using Company.App.Application.Interfaces;
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
        public IReadOnlyList<PurchaseOrderOverhead> Map(ExtractedDocumentDto document)
        {
            var lines = document.Lines;

            var targets = new[]
            {
            "1. PURCHASE ORDER REVISIONS",
            "2. PURCHASE ORDER DOCUMENTS",
            "3. SCOPE OF WORK:",
            "4. SPECIAL MARKING & PACKING REQUIREMENTS",
            "5. SPECIAL CONDITIONS OF PURCHASE",
            "6. SPECIAL ADMINISTRATIVE REQUIREMENTS",
            "7. CONTACT DETAILS FOR DOCUMENTATION AND COMMUNICATION:"
        };

            var overheadLines = ExtractedDocumentSearch
                .GetPagesByLineContent(lines, targets)
                .ToList();

            var sectionBlocks = ExtractedDocumentSearch
                .GetSectionBlocksFromLines(overheadLines)
                .ToList();

            var rootBlocks = sectionBlocks
                .Where(b => b.Section.Level == 1)
                .ToList();

            return rootBlocks
                .Select(root => BuildNode(root, sectionBlocks))
                .ToList();
        }

        /// <summary>
        /// Builds a hierarchical representation of a purchase order section and its subsections from the provided
        /// section data.
        /// </summary>
        /// <remarks>This method recursively processes the section hierarchy, associating each section
        /// with its direct children and filtering content to exclude lines belonging to child sections. The resulting
        /// structure can be used to represent nested purchase order details.</remarks>
        /// <param name="current">The section block representing the current node to build in the hierarchy.</param>
        /// <param name="allBlocks">A list of all section blocks available for constructing the hierarchy. Used to identify child sections and
        /// their relationships.</param>
        /// <returns>A PurchaseOrderOverhead instance representing the current section, including its content, bullet points, and
        /// recursively built subsections.</returns>
        private static PurchaseOrderOverhead BuildNode(
            SectionBlockDto current,
            List<SectionBlockDto> allBlocks)
        {
            var directChildren = allBlocks
                .Where(b => IsDirectChild(current.Section.SectionNumber, b.Section.SectionNumber))
                .ToList();

            var subSections = directChildren
                .Select(child => BuildNode(child, allBlocks))
                .ToList();

            var childNumbers = directChildren
                .Select(c => c.Section.SectionNumber)
                .ToHashSet();

            var ownLines = current.BlockLines
                .Skip(1)
                .Where(line =>
                {
                    var parsed = RegexSearchService.TryParseSectionFromLine(line.Text);
                    return parsed == null || !childNumbers.Contains(parsed.SectionNumber);
                })
                .ToList();

            var (content, bulletPoints) = SplitOwnContent(ownLines);

            return new PurchaseOrderOverhead(
                current.BlockLines.Select(x => x.PageNumber).Distinct().ToHashSet(),
                current.Section.SectionNumber,
                current.Section.Title,
                content,
                bulletPoints,
                subSections);
        }

        /// <summary>
        /// Determines whether the specified child represents a direct child of the given parent in a dot-delimited
        /// hierarchy.
        /// </summary>
        /// <remarks>A direct child is defined as a string that starts with the parent followed by a dot,
        /// and has exactly one additional segment in the hierarchy.</remarks>
        /// <param name="parent">The parent identifier, represented as a dot-delimited string.</param>
        /// <param name="child">The child identifier to evaluate, represented as a dot-delimited string.</param>
        /// <returns>true if child is a direct child of parent; otherwise, false.</returns>
        private static bool IsDirectChild(string parent, string child)
        {
            if (!child.StartsWith(parent + "."))
                return false;

            var parentLevel = parent.Count(c => c == '.') + 1;
            var childLevel = child.Count(c => c == '.') + 1;

            return childLevel == parentLevel + 1;
        }

        /// <summary>
        /// Splits a collection of extracted lines into separate lists of content and bullet points based on their
        /// formatting.
        /// </summary>
        /// <remarks>Lines that are null, empty, or consist only of whitespace are ignored. The method
        /// uses a bullet detection utility to classify lines as bullet points.</remarks>
        /// <param name="lines">The collection of extracted lines to process. Each line is evaluated to determine whether it is a bullet
        /// point or regular content.</param>
        /// <returns>A tuple containing two lists: the first list includes lines identified as regular content, and the second
        /// list includes lines identified as bullet points. Both lists are empty if no lines match their respective
        /// criteria.</returns>
        private static (List<string> Content, List<string> BulletPoints)
            SplitOwnContent(IEnumerable<ExtractedLineDto> lines)
        {
            var content = new List<string>();
            var bulletPoints = new List<string>();

            foreach (var line in lines)
            {
                var text = line.Text?.Trim();
                if (string.IsNullOrWhiteSpace(text))
                    continue;

                if (IsBulletLine.IsBullet(text))
                    bulletPoints.Add(text);
                else
                    content.Add(text);
            }

            return (content, bulletPoints);
        }
    }
}
