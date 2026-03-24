using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataExtraction.Models
{
    public class ExtractedLineDto
    {
        public int PageNumber { get; set; }
        public string Text { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
