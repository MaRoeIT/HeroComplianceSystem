namespace Company.App.Infrastructure.Adapters.Models
{
    /// <summary>
    /// Represents a single extracted word from a PDF document,
    /// including its text content and placement on the page.
    /// </summary>
    internal class PdfWordModel
    {
        // Gets or sets the extracted text value of the word.
        public string Text { get; set; } = string.Empty;

        // Gets or sets the page number where the word was found.
        public int PageNumber { get; set; }

        // Gets or sets the horizontal position of the word.
        public double X { get; set; }

        // Gets or sets the vertical position of the word.
        public double Y { get; set; }

        // Gets or sets the width of the word's bounding box.
        public double Width { get; set; }

        // Gets or sets the height of the word's bounding box.
        public double Height { get; set; }
    }
}
