namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents a Material Documentation Package document
    /// within the OneSubSea document domain.
    /// </summary>
    public record MaterialDocumentationPackage : Order
    {
        public MaterialDocumentationPackageHeader Header { get; } = new();
        public MaterialDocumentationPackageIndex Index { get; } = new();

        public MaterialDocumentationPackage(
        string orderNumber,
        string orderDate
        ) : base(orderNumber, orderDate) { }
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

    public record MaterialDocumentationPackageIndex
    {   
        public List<MaterialMasterReport> MaterialMasterReports { get; } = new();

    }
    public record MaterialMasterReport
    {
        public string ReportDate{ get; } = string.Empty;
        public string MaterialNumber { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public string RevisionLevel { get; } = string.Empty;
        public string Plant { get; } = string.Empty;
        public string BaseUnitOfMeasure { get; } = string.Empty;
        public string GrossWeight { get; } = string.Empty;
        public string NetWeight { get; } = string.Empty;
        public string BasicMaterial { get; } = string.Empty;
        public string CommodityCode { get; } = string.Empty;
        public string ExportControlCode { get; } = string.Empty;
        public string Manufacturer { get; } = string.Empty;
        public string ManufacturerPartNumber { get; } = string.Empty;
        public string MIRID { get; } = string.Empty;
        public string MIRTitle { get; } = string.Empty;
        public string MIRDescription { get; } = string.Empty;
        public string Criticality { get; } = string.Empty;
        public string TraceabilityType { get; } = string.Empty;
        public string SerialNumberProfile { get; } = string.Empty;
        public string ShelfLife { get; } = string.Empty;
        public string QMControlKey { get; } = string.Empty;
        public List<InspectionSetup> InspectionSetups { get; } = new List<InspectionSetup>();
        public List<Classification> Classifications { get; } = new List<Classification>();
        public List<OtherRelatedDoc> OtherRelatedDocs { get; } = new List<OtherRelatedDoc>();
    }
    public record InspectionSetup
    {
        public string InspectionSetupItem { get; } = string.Empty;
    }
    public record Classification
    {
        public string ClassificationItem { get; } = string.Empty;
    }
    public record OtherRelatedDoc 
    {
        public string OtherRelatedDocItem { get; } = string.Empty;
    }

}
