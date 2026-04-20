using System;
using System.Collections.Generic;
using System.Text;
using Company.App.Domain.Entities.OneSubSea;
using Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements;
using Company.App.Application.UseCases.DataExtraction.Models;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.AdministrativeRequirementsDocument
{
    public sealed class MapAppendicesHeader : IAppendicesHeaderMapper
    {
        public IReadOnlyList<AppendicesHeader> Map(ExtractedDocumentDto document)
        {
            var words = document.Words;

            var firstPage = GetLinesOnPage(document.Lines, 1);

            var documentTitle = string.Join(" ", firstPage
                .Where(l =>
                    l.Y <= 700 &&
                    l.Y >= 600)
                .Select(l => l.Text));

            var headWords = GetWordsFromLines(words, firstPage)
                    .Where(w =>
                        w.Y <= 750 &&
                        w.Y >= 700)
                    .OrderBy(w => w.X)
                    .ToList();

            var documentId = firstPage
                .Where(l => l.Y <= 580 && l.Y >= 540)
                .Select(l => l.Text)
                .FirstOrDefault();

            var revisionNumber = headWords
                .Where(w => w.X <= 70)
                .Select(w => w.Text)
                .FirstOrDefault();

            var status = string.Join(" ", headWords
                .Where(w =>
                    w.X >= 70 &&
                    w.X <= 150)
                .Select(w => w.Text));

            var dateLines = firstPage
                .Where(l =>
                    l.Y <= 750 &&
                    l.Y >= 700);

            var parsedIssueDate = ParseDateFromLines(dateLines);

            if (parsedIssueDate is null)
            {
                var debugLines = string.Join(" | ", dateLines.Select(l => l.Text));

                throw new InvalidOperationException(
                    $"Could not parse issue date from appendix document. Header lines: {debugLines}");
            }

            var businessSegment = string.Empty;

            var businessProcess = string.Empty;

            var owner = RemoveSymbolsFromString(string.Join(" ", headWords
                .Where(w =>
                    w.X >= 420 &&
                    w.X <= 520)
                .Select(w => w.Text)));

            var approver = RemoveSymbolsFromString(string.Join(" ", headWords
                .Where(w =>
                    w.X >= 340 &&
                    w.X <= 400)
                .Select(w => w.Text)));

            var author = RemoveSymbolsFromString(string.Join(" ", headWords
                .Where(w =>
                    w.X >= 250 &&
                    w.X <= 290)
                .Select(w => w.Text)));

            return new List<AppendicesHeader>
            {
                new AppendicesHeader(
                documentTitle,
                documentId,
                revisionNumber,
                status,
                parsedIssueDate,
                businessSegment,
                businessProcess,
                owner,
                approver,
                author)
            };
        }
    }
}
