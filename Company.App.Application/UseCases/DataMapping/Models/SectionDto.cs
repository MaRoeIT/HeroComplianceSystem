using Company.App.Application.UseCases.DataExtraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    public record SectionDto
    (
        string SectionNumber,
        string Title,
        int Level
    );

    public sealed record SectionBlockDto(
    SectionDto Section,
    List<ExtractedLineDto> BlockLines);
}
