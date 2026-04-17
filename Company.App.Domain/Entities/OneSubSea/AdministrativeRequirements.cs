namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents an Administrative Requirements document
    /// within the OneSubSea document domain.
    /// </summary>
    public record AdministrativeRequirements : Order
    {
        public AdministrativeRequirementsHeader Header { get; }

        public AdministrativeRequirementsOverHead OverHead { get; }

        public AdministrativeRequirements(
            AdministrativeRequirementsHeader header,
            AdministrativeRequirementsOverHead overHead,
            string orderNumber,
            string orderDate
            ) : base(orderNumber, orderDate)
        {
            Header = header;
            OverHead = overHead;
        }
    }

    public record AdministrativeRequirementsHeader
    {
        public string DocumentId { get; }

        public string RevisionNumber { get; }

        public DateOnly IssueDate { get; }
        
        public string BusinessSegment { get; }

        public string BusinessProcess { get; }

        public string Owner { get; }
        
        public string Approver { get; }
        
        public string Author { get; }
    }

    public record AdministrativeRequirementsOverHead
    {
        public HashSet<int> NumberOfPages { get; }

        public HashSet<int> NumberOfDocuments { get; }

        public IReadOnlyList<string> DocumentList { get; }

        public AdministrativeRequirementsOverHead(
            HashSet<int> numberOfPages,
            HashSet<int> numberOfDocuments,
            IReadOnlyList<string> documentList)
        {
            NumberOfPages = numberOfPages;
            NumberOfPages = numberOfPages;
            DocumentList = documentList;
        }
    }

    public record AdministrativeRequirementsSection
    {
        public string Title { get; }

        public HashSet<int> Pages { get; }

        public IReadOnlyList<string> Content { get; }

        public IReadOnlyList<AdministrativeRequirementsSection> SubSections { get; }

        public AdministrativeRequirementsSection(
            string title,
            HashSet<int> pages,
            IReadOnlyList<string> content,
            IReadOnlyList<AdministrativeRequirementsSection> subSections
            )
        {
            Title = title;
            Pages = pages;
            Content = content;
            SubSections = subSections;
        }
    }
}
