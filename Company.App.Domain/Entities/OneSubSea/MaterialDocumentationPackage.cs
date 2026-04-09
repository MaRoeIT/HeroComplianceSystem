namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents a Material Documentation Package document
    /// within the OneSubSea document domain.
    /// </summary>
    public record MaterialDocumentationPackage : Order
    {
        public MaterialDocumentationPackageHeader Header { get; } = new();

    }
    public record MaterialDocumentationPackageHeader
    {
        public string DocumentTitle { get; } = string.Empty;  
        public string PurchaseOrderDate { get; } = string.Empty;
        public string MDPIssueDateTime { get; } = string.Empty;
        public string DocumentRevision { get; } = string.Empty;
        public string OurReference { get; } = string.Empty;
        public string BuyerTelephoneNumber { get; } = string.Empty;
        public string Email { get; } = string.Empty;
    }
}
