using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataExtraction.Models
{
    /// <summary>
    /// Represents a single extracted text line from a PDF page,
    /// including positional information.
    /// </summary>
    public record ExtractedLineDto
    {
        // The page number where the line was found.
        public int PageNumber { get; set; }
        
        // The textual content of the extracted line.
        public string Text { get; set; } = string.Empty;

        // The horizontal position of the line in the PDF coordinate space.
        public double X { get; set; }

        // The vertical position of the line in the PDF coordinate space.
        public double Y { get; set; }

        // The width of the extracted line area.
        public double Width { get; set; }

        // The height of the extracted line area.
        public double Height { get; set; }
    }
}
