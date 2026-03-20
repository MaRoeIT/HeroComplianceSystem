using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using Company.App.Application.UseCases.DataExtraction;

namespace Company.App.Infrastructure.DataExtractor
{
    public class PdfWordExtractor
    {
        public PdfDocumentResult Extract(string path)
        {
            var result = new PdfDocumentResult
            {
                FilePath = path
            };

            using (PdfDocument document = PdfDocument.Open(path))
            {
                foreach (Page page in document.GetPages())
                {
                    var pageWords = ExtractWordsFromPage(page).ToList();
                    result.Words.AddRange(pageWords);

                    var pageLines = GroupWordsIntoLines(pageWords, page.Number);
                    result.Lines.AddRange(pageLines);
                }
            }

            return result;
        }

        private IEnumerable<PdfWordModel> ExtractWordsFromPage(Page page)
        {
            foreach (Word word in page.GetWords())
            {
                var box = word.BoundingBox;

                yield return new PdfWordModel
                {
                    Text = word.Text,
                    PageNumber = page.Number,
                    X = box.Left,
                    Y = box.Bottom,
                    Width = box.Width,
                    Height = box.Height
                };
            }
        }

        private List<PdfLineModel> GroupWordsIntoLines(List<PdfWordModel> words, int pageNumber)
        {
            var lines = new List<PdfLineModel>();

            if (words.Count == 0)
            {
                return lines;
            }

            // Simple line grouping by Y-position tolerance
            const double yTolerance = 3.0;

            var grouped = words
                .OrderByDescending(w => w.Y)
                .ThenBy(w => w.X)
                .GroupBy(w => FindLineKey(w.Y, yTolerance))
                .OrderByDescending(g => g.Key);

            foreach (var group in grouped)
            {
                var orderedWords = group.OrderBy(w => w.X).ToList();

                //Calculate line box.
                var MinX = orderedWords.Min(w => w.X);
                var MaxX = orderedWords.Max(w => w.X + w.Width);
                var MinY = orderedWords.Min(w => w.Y);
                var MaxY = orderedWords.Max(w => w.Y + w.Height);

                lines.Add(new PdfLineModel
                {
                    PageNumber = pageNumber,
                    Words = orderedWords,
                    Text = string.Join(" ", orderedWords.Select(w => w.Text)),
                    X = MinX,
                    Y = MinY,
                    Width = MaxX - MinX,
                    Height = MaxY - MinY
                });
            }

            return lines;
        }

        private double FindLineKey(double y, double tolerance)
        {
            return Math.Round(y / tolerance) * tolerance;
        }
    }
}