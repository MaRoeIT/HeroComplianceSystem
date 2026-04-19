namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents an Administrative Requirements document
    /// within the OneSubSea document domain.
    /// </summary>
    public record AdministrativeRequirements
    {
        public string DocumentId { get; }
        public DateOnly IssueDate { get; }
        public AdministrativeRequirementsHeader Header { get; }

        //public IReadOnlyList<AppendicesHeader> Appendices { get; }

        public AdministrativeRequirements(
            string documentId,
            DateOnly issueDate,
            AdministrativeRequirementsHeader header
            //IReadOnlyList<AppendicesHeader> appendices
            )
        {
            DocumentId = documentId;
            IssueDate = issueDate;
            Header = header;
            //Appendices = appendices;
        }
    }

    public record AdministrativeRequirementsHeader
    {
        public string DocumentId { get; }

        public string RevisionNumber { get; }

        public string? Status { get; }

        public DateOnly IssueDate { get; }
        
        public string? BusinessSegment { get; }

        public string? BusinessProcess { get; }

        public string Owner { get; }
        
        public string Approver { get; }
        
        public string Author { get; }

        public AdministrativeRequirementsHeader(
            string documentId,
            string revisionNumber,
            string? status,
            DateOnly issuedate,
            string? businessSegment,
            string? businessProcess,
            string owner,
            string approver,
            string author)
        {
            DocumentId = documentId;
            RevisionNumber = revisionNumber;
            Status = status;
            IssueDate = issuedate;
            BusinessSegment = businessSegment;
            BusinessProcess = businessProcess;
            Owner = owner;
            Approver = approver;
            Author = author;
        }
    }

    public record AppendicesHeader : AdministrativeRequirementsHeader
    {
        public string DocumentTitle { get; }
        public AppendicesHeader(
            string documentTitle,
            string documentId,
            string revisionNumber,
            string? status,
            DateOnly issuedate,
            string? businessSegment,
            string? businessProcess,
            string owner,
            string approver,
            string author)
            : base(documentId, revisionNumber, status, issuedate, businessSegment, businessProcess, owner, approver, author)
        {
            DocumentTitle = documentTitle;
        }
    }

    /*
    // Potential solution for storing section data fro AR objects.
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
            SubSections = subSections ?? Array.Empty<AdministrativeRequirementsSection>();
        }
    }
    */
}
