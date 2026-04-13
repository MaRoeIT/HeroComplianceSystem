namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents a Material Documentation Package document
    /// within the OneSubSea document domain.
    /// </summary>
    public record MaterialDocumentationPackage : Order
    {
        public MaterialDocumentationPackageHeader Header { get; }
        public MaterialDocumentationPackageIndex Index { get; }

        public MaterialDocumentationPackage(
        string orderNumber,
        string orderDate,
        MaterialDocumentationPackageHeader header
        ) : base(orderNumber, orderDate)
        {
            Header = header;
        }
        }
    public record MaterialDocumentationPackageHeader
    {
        public string DocumentTitle { get; }
        public string PurchaseOrderDate { get; }
        public string MDPIssueDateTime { get; }
        public string DocumentRevision { get; }
        public string OurReference { get; }
        public string BuyerTelephoneNumber { get; }
        public string Email { get; }
    }

    public record MaterialDocumentationPackageIndex
    {   
        public IReadOnlyList<MaterialMasterReport> MaterialMasterReports { get; }

    }
    public record MaterialMasterReport
    {
        public IReadOnlyList<MaterialReport> MaterialReports { get; }
    }
    public record MaterialReport
    {
        public string ReportDate{ get; }
        public string MaterialNumber { get; }
        public string Description { get; }
        public string RevisionLevel { get; }
        public string Plant { get; }
        public string BaseUnitOfMeasure { get; }
        public string GrossWeight { get; }
        public string NetWeight { get; }
        public string BasicMaterial { get; }
        public string CommodityCode { get; }
        public string ExportControlCode { get; }
        public string Manufacturer { get; }
        public string ManufacturerPartNumber { get; }
        public string MIRID { get; }
        public string MIRTitle { get; }
        public string MIRDescription { get; }
        public string Criticality { get; }
        public string TraceabilityType { get; }
        public string SerialNumberProfile { get; }
        public string ShelfLife { get; }
        public string QMControlKey { get; }
        public IReadOnlyList<InspectionSetup> InspectionSetups { get; }
        public IReadOnlyList<Classification> Classifications { get; }
        public IReadOnlyList<OtherRelatedDocs> OtherRelatedDocs { get; }
    }
    public record InspectionSetup
    {
        public string InspectionSetupItem { get; }
    }
    public record Classification
    {
        public string ClassificationItem { get; }
    }
    public record OtherRelatedDocs
    {
        public IReadOnlyList<string> OtherRelatedDocsItem { get; }
    }

}
