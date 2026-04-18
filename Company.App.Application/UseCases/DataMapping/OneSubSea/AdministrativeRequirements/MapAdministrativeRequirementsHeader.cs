using System;
using System.Collections.Generic;
using System.Text;
using Company.App.Domain.Entities.OneSubSea;
using Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements;
using Company.App.Application.UseCases.DataExtraction.Models;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea.AdministrativeRequirements
{
    public sealed class MapAdministrativeRequirementsHeader : IAdministrativeRequirementsHeaderMapper
    {
        public AdministrativeRequirementsHeader Map(ExtractedDocumentDto document)
        {
            var words = document.Words;

            var firstPage = GetLinesOnPage(document.Lines, 1);
            var controllLine = GetFirstLineContaining(firstPage, "TABLE OF CONTENTS");

            if (controllLine == null)
            {
                var lines = GetLinesOnPages(document.Lines, 1, 2);
                var date1 = ParseDateFromLines(GetLinesFromTargetLine(lines, "Status Issue date", 3));
                var date2 = ParseDateFromLine(GetFirstLineContaining(lines, "Effective Date"));

                IEnumerable<ExtractedLineDto> headLiners;
                IEnumerable<ExtractedWordDto> headWords;

                if (date1 > date2)
                {
                    headLiners = GetPagesByLineContent(lines, ["Status Issue date"]);

                    headWords = GetWordsFromLines(words, headLiners)
                        .Where(w => w.Y <= 780 && w.Y >= 750)
                        .OrderBy(w => w.X)
                        .ThenByDescending(w => w.Y);

                    var documentId = headLiners
                        .Where(l => l.Y >= 340 && l.Y <= 380)
                        .Select(l => l.Text)
                        .FirstOrDefault();

                    var revisionNumber = headWords
                        .Where(w => w.X <= 70)
                        .Select(w => w.Text)
                        .FirstOrDefault();

                    var status = string.Join(" ", headWords
                        .Where(w => w.X >= 70 && w.X <= 140)
                        .Select(w => w.Text));

                    var IssueDate = (DateOnly)date1;

                    var businessSegment = string.Empty;

                    var businessProcess = string.Empty;

                    var owner = string.Join(" ", headWords
                        .Where(w => w.X >= 440 && w.X <= 520)
                        .Select(w => w.Text));

                    var approver = string.Join(" ", headWords
                        .Where(w => w.X >= 350 && w.X <= 410));

                    var author = string.Join(" ", headWords
                        .Where(w => w.X >= 250 && w.X <= 290)
                        .Select(w => w.Text));

                    return new AdministrativeRequirementsHeader(
                        documentId,
                        revisionNumber,
                        status,
                        IssueDate,
                        businessSegment,
                        businessProcess,
                        owner,
                        approver,
                        author);


                }
                else
                {
                    headLiners = GetPagesByLineContent(lines, ["Effective Date"])
                        .Where(l => l.Y >= 400 && l.Y <= 600);

                    foreach (var l in headLiners)
                        Console.WriteLine($"{l.Y},{l.Text}");
                }
            }
            else
                Console.WriteLine("SkipIf");

        }
    }
}
