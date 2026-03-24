using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Infrastructure.Adapters.Models
{
    internal class PdfLineModel
    {
        public int PageNumber { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<PdfWordModel> Words { get; set; } = new();

        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
