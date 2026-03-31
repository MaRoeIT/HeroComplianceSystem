using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataExtraction.Models
{
    /// <summary>
    /// Represents a single extracted word from a PDF page,
    /// including positional information.
    /// </summary>
    public record ExtractedWordDto
    {
        // The page number where the word was found.
        public int PageNumber { get; set; }

        // The textual value of the extracted word.
        public string Text { get; set; } = string.Empty;

        // The horizontal position of the word in the PDF coordinate space.
        public double X { get; set; }

        // The vertical position of the word in the PDF coordinate space.
        public double Y { get; set; }

        // The width of the extracted word area.
        public double Width { get; set; }

        // The height of the extracted word area.
        public double Height { get; set; }
    }
}
