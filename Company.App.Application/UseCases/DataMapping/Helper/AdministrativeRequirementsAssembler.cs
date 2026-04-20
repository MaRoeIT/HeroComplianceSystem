using Company.App.Domain.Entities.OneSubSea;
using System.Collections.Generic;
using System.Linq;

namespace Company.App.Application.UseCases.DataMapping.Helper
{
    public static class AdministrativeRequirementsAssembler
    {
        public static AdministrativeRequirements AttachAppendices(
            AdministrativeRequirements parent,
            IEnumerable<AppendicesHeader> appendices)
        {
            var appendixList = (parent.Appendices ?? Array.Empty<AppendicesHeader>())
                .Concat(appendices)
                .ToList();

            return new AdministrativeRequirements(
                parent.DocumentId,
                parent.IssueDate,
                parent.Header,
                appendixList
            );
        }
    }
}