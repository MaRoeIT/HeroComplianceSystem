using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Infrastructure.Adapters.Models
{
    internal class PdfDocumentResult
    {
        public string FilePath { get; set; } = string.Empty;

        public List<PdfWordModel> Words { get; set; } = new();
        public List<PdfLineModel> Lines { get; set; } = new();
    }
}
