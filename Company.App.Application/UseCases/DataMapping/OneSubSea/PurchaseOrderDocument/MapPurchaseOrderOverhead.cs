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
    public sealed class MapPurchaseOrderOverhead : IPurchaseOrderOverheadMapper
    {
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

        private static bool IsDirectChild(string parent, string child)
        {
            if (!child.StartsWith(parent + "."))
                return false;

            var parentLevel = parent.Count(c => c == '.') + 1;
            var childLevel = child.Count(c => c == '.') + 1;

            return childLevel == parentLevel + 1;
        }

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
