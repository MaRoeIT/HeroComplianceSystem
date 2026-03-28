using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Infrastructure.Adapters.Models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Company.App.Infrastructure.Adapters
{
    /// <summary>
    /// PDF extraction adapter that uses PdfPig to read textual content
    /// and positional data from PDF documents.
    /// </summary>
    public class PdfPigDataExtractor : IPdfDataExtractor
    {
        /// <summary>
        /// Extracts word-level and line-level text data from a PDF file.
        /// </summary>
        /// <param name="fileData">The raw PDF file content as a byte array.</param>
        /// <returns>
        /// An extracted document DTO containing all discovered words and grouped lines.
        /// </returns>
        public async Task<ExtractedDocumentDto> ExtractPdfData(byte[] fileData)
        {
            return await Task.Run(() =>
            {
                using var stream = new MemoryStream(fileData);
                var result = new ExtractedDocumentDto();

                // Open the PDF document from the provided in-memory stream.
                using var document = PdfDocument.Open(stream);

                // Process each page independently so both page number
                // and page-local layout information are preserved.
                foreach (Page page in document.GetPages())
                {
                    // Extract all words from the current page, including coordinates.
                    var pageWords = ExtractWordsFromPage(page).ToList();

                    // Map infrastructure models to application DTOs.
                    result.Words.AddRange(pageWords.Select(w => new ExtractedWordDto
                    {
                        Text = w.Text,
                        PageNumber = w.PageNumber,
                        X = w.X,
                        Y = w.Y,
                        Width = w.Width,
                        Height = w.Height
                    }));

                    // Group nearby words into logical text lines.
                    var pageLines = GroupWordsIntoLines(pageWords, page.Number);

                    // Map grouped lines to application DTOs.
                    result.Lines.AddRange(pageLines.Select(l => new ExtractedLineDto
                    {
                        Text = l.Text,
                        PageNumber = l.PageNumber,
                        X = l.X,
                        Y = l.Y,
                        Width = l.Width,
                        Height = l.Height
                    }));
                }

                return result;
            });
        }

        /// <summary>
        /// Extracts individual words from a single PDF page
        /// together with their bounding box coordinates.
        /// </summary>
        /// <param name="page">The PDF page to extract words from.</param>
        /// <returns>A sequence of extracted word models.</returns>
        private IEnumerable<PdfWordModel> ExtractWordsFromPage(Page page)
        {
            foreach (Word word in page.GetWords())
            {
                var box = word.BoundingBox;

                // Store the visible text and its geometric placement on the page.
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

        /// <summary>
        /// Groups words into lines by clustering words that are positioned
        /// close to each other on the Y-axis.
        /// </summary>
        /// <param name="words">The extracted words from one page.</param>
        /// <param name="pageNumber">The page number the words belong to.</param>
        /// <returns>A list of reconstructed line models.</returns>
        private List<PdfLineModel> GroupWordsIntoLines(List<PdfWordModel> words, int pageNumber)
        {
            var lines = new List<PdfLineModel>();

            if (words.Count == 0)
            {
                return lines;
            }

            // Controls how close words must be vertically
            // to be considered part of the same line.
            const double yTolerance = 3.0;

            var grouped = words
                .OrderByDescending(w => w.Y)
                .ThenBy(w => w.X)
                .GroupBy(w => FindLineKey(w.Y, yTolerance))
                .OrderByDescending(g => g.Key);

            foreach (var group in grouped)
            {
                // Preserve natural left-to-right reading order inside the line.
                var orderedWords = group.OrderBy(w => w.X).ToList();

                // Compute a bounding box covering the full reconstructed line.
                var minX = orderedWords.Min(w => w.X);
                var maxX = orderedWords.Max(w => w.X + w.Width);
                var minY = orderedWords.Min(w => w.Y);
                var maxY = orderedWords.Max(w => w.Y + w.Height);

                lines.Add(new PdfLineModel
                {
                    PageNumber = pageNumber,
                    Words = orderedWords,

                    // Rebuild line text by joining extracted words with spaces.
                    Text = string.Join(" ", orderedWords.Select(w => w.Text)),
                    X = minX,
                    Y = minY,
                    Width = maxX - minX,
                    Height = maxY - minY
                });
            }

            return lines;
        }

        /// <summary>
        /// Converts a Y-position to a normalized grouping key.
        /// This is used to cluster nearby words into the same logical line.
        /// </summary>
        /// <param name="y">The original Y-coordinate.</param>
        /// <param name="tolerance">The vertical grouping tolerance.</param>
        /// <returns>A normalized line key.</returns>
        private double FindLineKey(double y, double tolerance)
        {
            return Math.Round(y / tolerance) * tolerance;
        }
    }
}