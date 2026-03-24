using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Infrastructure.Adapters.Models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Company.App.Infrastructure.Adapters
{
    public class PdfPigDataExtractor : IPdfDataExtractor
    {
        public ExtractedDocumentDto Extract(Stream stream)
        {
            var result = new ExtractedDocumentDto();

            using var document = PdfDocument.Open(stream);

            foreach (Page page in document.GetPages())
            {
                var pageWords = ExtractWordsFromPage(page).ToList();
                result.Words.AddRange(pageWords.Select(w => new ExtractedWordDto
                {
                    Text = w.Text,
                    PageNumber = w.PageNumber,
                    X = w.X,
                    Y = w.Y,
                    Width = w.Width,
                    Height = w.Height,
                }));

                var pageLines = GroupWordsIntoLines(pageWords, page.Number);
                result.Lines.AddRange(pageLines.Select(l => new ExtractedLineDto
                {
                    Text = l.Text,
                    PageNumber = l.PageNumber,
                    X = l.X,
                    Y = l.Y,
                    Width = l.Width,
                    Height= l.Height,
                }));
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
