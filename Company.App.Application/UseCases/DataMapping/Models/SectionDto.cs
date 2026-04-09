using Company.App.Application.UseCases.DataExtraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.DataMapping.Models
{
    /// <summary>
    /// Represents a data transfer object for a document section, including its number, title, and hierarchical level.
    /// </summary>
    /// <param name="SectionNumber">The unique identifier or number assigned to the section. Cannot be null.</param>
    /// <param name="Title">The title or heading of the section. Cannot be null.</param>
    /// <param name="Level">The hierarchical level of the section within the document structure. Must be zero or greater, where zero
    /// typically represents the top-level section.</param>
    public record SectionDto
    (
        string SectionNumber,
        string Title,
        int Level
    );

    /// <summary>
    /// Represents a block of lines that belong to a specific section.
    /// </summary>
    /// <param name="Section">The section to which the block of lines belongs. Cannot be null.</param>
    /// <param name="BlockLines">The collection of extracted lines that make up the block. Cannot be null.</param>
    public sealed record SectionBlockDto(
    SectionDto Section,
    List<ExtractedLineDto> BlockLines);
}
