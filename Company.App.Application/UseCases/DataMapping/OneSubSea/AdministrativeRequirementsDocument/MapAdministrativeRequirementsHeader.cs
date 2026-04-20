using System;
using System.Collections.Generic;
using System.Text;
using Company.App.Domain.Entities.OneSubSea;
using Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements;
using Company.App.Application.UseCases.DataExtraction.Models;
using static Company.App.Application.UseCases.DataMapping.Services.ExtractedDocumentSearch;

namespace Company.App.Application.UseCases.DataMapping.OneSubSea
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
                        .Where(w =>
                            w.Y <= 780 &&
                            w.Y >= 750)
                        .OrderBy(w => w.X)
                        .ThenByDescending(w => w.Y);

                    var documentId = headLiners
                        .Where(l =>
                        l.Y >= 340 &&
                        l.Y <= 380)
                        .Select(l => l.Text)
                        .FirstOrDefault();

                    var revisionNumber = headWords
                        .Where(w => w.X <= 70)
                        .Select(w => w.Text)
                        .FirstOrDefault();

                    var status = string.Join(" ", headWords
                        .Where(w =>
                        w.X >= 70 &&
                        w.X <= 140)
                        .Select(w => w.Text));

                    var issueDate = date1;

                    var businessSegment = string.Empty;

                    var businessProcess = string.Empty;

                    var owner = RemoveSymbolsFromString(string.Join(" ", headWords
                        .Where(w =>
                            w.X >= 440 &&
                            w.X <= 520)
                        .Select(w => w.Text)));

                    var approver = RemoveSymbolsFromString(string.Join(" ", headWords
                        .Where(w =>
                            w.X >= 350 &&
                            w.X <= 410)
                        .Select(w => w.Text)));

                    var author = RemoveSymbolsFromString(string.Join(" ", headWords
                        .Where(w =>
                            w.X >= 250 &&
                            w.X <= 290)
                        .Select(w => w.Text)));

                    return new AdministrativeRequirementsHeader(
                        documentId,
                        revisionNumber,
                        status,
                        issueDate,
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

                    var documentId = GetValueAfterLabel(headLiners, "Document");

                    var revisionNumber = GetValueAfterLabel(headLiners, "Revision");

                    var status = string.Empty;

                    var issueDate = date2;

                    var businessSegment = GetValueAfterLabel(headLiners, "Business Segment");

                    var businessProcess = GetValueAfterLabel(headLiners, "Business Process");

                    var owner = GetValueAfterLabel(headLiners, "Owner");

                    var approver = GetValueAfterLabel(headLiners, "Approver");

                    var author = GetValueAfterLabel(headLiners, "Author");

                    return new AdministrativeRequirementsHeader(
                        documentId,
                        revisionNumber,
                        status,
                        issueDate,
                        businessSegment,
                        businessProcess,
                        owner,
                        approver,
                        author);
                }
            }
            else
            {
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

                var issueDate = (DateOnly)ParseDateFromLines(firstPage
                    .Where(l =>
                        l.Y <= 750 &&
                        l.Y >= 700));

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

                return new AdministrativeRequirementsHeader(
                    documentId,
                    revisionNumber,
                    status,
                    issueDate,
                    businessSegment,
                    businessProcess,
                    owner,
                    approver,
                    author);
            }
        }
    }
}
