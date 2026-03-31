namespace Company.App.Infrastructure.Adapters.Models
{
    /// <summary>
    /// Represents a reconstructed text line in a PDF document,
    /// built from one or more extracted words.
    /// </summary>
    internal class PdfLineModel
    {
        // Gets or sets the page number where the line was found.
        public int PageNumber { get; set; }

        // Gets or sets the combined text content of the line.
        public string Text { get; set; } = string.Empty;

        // Gets or sets the words that were grouped into this line.
        public List<PdfWordModel> Words { get; set; } = new();

        // Gets or sets the horizontal start position of the line.
        public double X { get; set; }

        // Gets or sets the vertical start position of the line.
        public double Y { get; set; }

        // Gets or sets the width of the line's bounding area.
        public double Width { get; set; }

        // Gets or sets the height of the line's bounding area.
        public double Height { get; set; }
    }
}
