using System;
using System.Collections.Generic;
using System.Text;
using Company.App.Infrastructure.Persistents.Entities;

namespace Company.App.Application.UseCases.DataExtraction
{
    public class PdfWordModel
    {
        public string Text { get; set; } = string.Empty;
        public int PageNumber { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
    }
    public class PdfLineModel
    {
        public int PageNumber { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<PdfWordModel> Words { get; set; } = new();

        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
    public class PdfDocumentResult
    {
        public string FilePath { get; set; } = string.Empty;

        public List<PdfWordModel> Words { get; set; } = new();
        public List<PdfLineModel> Lines { get; set; } = new();
    }
}