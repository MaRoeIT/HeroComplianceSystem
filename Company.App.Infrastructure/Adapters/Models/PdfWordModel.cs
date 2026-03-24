using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Infrastructure.Adapters.Models
{
    internal class PdfWordModel
    {
        public string Text { get; set; } = string.Empty;
        public int PageNumber { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
    }
}
