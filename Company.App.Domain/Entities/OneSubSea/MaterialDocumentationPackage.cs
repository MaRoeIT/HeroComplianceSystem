using System.Reflection.PortableExecutable;

namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents a Material Documentation Package document
    /// within the OneSubSea document domain.
    /// </summary>
    public record MaterialDocumentationPackage : Order
    {
        public MaterialDocumentationPackageHeader Header { get; }
        public IReadOnlyList<MaterialReport> MaterialReports { get; }
        public RelatedDocuments RelatedDocuments { get; }

        public MaterialDocumentationPackage(
        string orderNumber,
        DateOnly? orderDate,
        MaterialDocumentationPackageHeader header,
        IReadOnlyList<MaterialReport> materialReports
        ) : base(orderNumber, orderDate)
        {
            Header = header;
            MaterialReports = materialReports;
        }
    }

    public record MaterialDocumentationPackageHeader
    {
        public string PurchaseOrderDate { get; }
        public string MdpIssueDateTime { get; }
        public string DocumentRevision { get; }
        public string OurReference { get; }
        public string Buyer { get; }
        public string TelephoneNumber { get; }
        public string Email { get; }

        public MaterialDocumentationPackageHeader(
            string purchaseOrderDate,
            string mdpIssueDateTime,
            string documentRevision,
            string ourReference,
            string buyer,
            string telephoneNumber,
            string email)
        {
            PurchaseOrderDate = purchaseOrderDate;
            MdpIssueDateTime = mdpIssueDateTime;
            DocumentRevision = documentRevision;
            OurReference = ourReference;
            Buyer = buyer;
            TelephoneNumber = telephoneNumber;
            Email = email;
        }
    }

    public record MaterialReport
    {
        public MaterialReportHeader Header { get; }
        public MaterialReportOverHead Overhead { get; }
        public MaterialReportCharacteristics Characteristics { get; }

        public MaterialReport(
            MaterialReportHeader header,
            MaterialReportOverHead overhead,
            MaterialReportCharacteristics characteristics)
        {
            Header = header;
            Overhead = overhead;
            Characteristics = characteristics;
        }
    }

    public record MaterialReportHeader
    {
        public HashSet<int> PageNumber { get; }
        public string ReportDate { get; }
        public string MaterialNumber { get; }
        public string Description { get; }
        public string RevisionLevel { get; }
        public string Plant { get; }

        public MaterialReportHeader(
            string reportDate,
            string materialNumber,
            string description,
            string revisionLevel,
            string plant)
        {
            ReportDate = reportDate;
            MaterialNumber = materialNumber;
            Description = description;
            RevisionLevel = revisionLevel;
            Plant = plant;
        }
    }

    public record MaterialReportOverHead
    {
        
        public string BaseUnitOfMeasure { get; }
        public string GrossWeight { get; }
        public string NetWeight { get; }
        public string BasicMaterial { get; }
        public string CommodityCode { get; }
        public string ExportControlCode { get; }
        public string Manufacturer { get; }
        public string ManufacturerPartNumber { get; }
        public string MirId { get; }
        public string MirTitle { get; }
        public string MirDescription { get; }
        public string Criticality { get; }
        public string TraceabilityType { get; }
        public string SerialNumberProfile { get; }
        public string ShelfLife { get; }
        public string QMControlKey { get; }
        public IReadOnlyList<string> InspectionSetups { get; }
        public IReadOnlyList<string> Classifications { get; }
        public IReadOnlyList<string> OtherRelatedDocs { get; }
        public BasicDataText BasicDataText { get; }

        public MaterialReportOverHead(
            string baseUnitOfMeasure,
            string grossWeight,
            string netWeight,
            string basicMaterial,
            string commodityCode,
            string exportControlCode,
            string manufacturer,
            string manufacturerPartNumber,
            string mirid,
            string mirTitle,
            string mirDescription,
            string criticality,
            string traceabilityType,
            string serialNumberProfile,
            string shelfLife,
            string qmControlKey,
            IReadOnlyList<string> inspectionSetups,
            IReadOnlyList<string> classifications,
            IReadOnlyList<string> otherRelatedDocs,
            IReadOnlyList<string> basicDataTexts,
            BasicDataText basicDataText)
        {
            BaseUnitOfMeasure = baseUnitOfMeasure;
            GrossWeight = grossWeight;
            NetWeight = netWeight;
            BasicMaterial = basicMaterial;
            CommodityCode = commodityCode;
            ExportControlCode = exportControlCode;
            Manufacturer = manufacturer;
            ManufacturerPartNumber = manufacturerPartNumber;
            MirId = mirid;
            MirTitle = mirTitle;
            MirDescription = mirDescription;
            Criticality = criticality;
            TraceabilityType = traceabilityType;
            SerialNumberProfile = serialNumberProfile;
            ShelfLife = shelfLife;
            QMControlKey = qmControlKey;
            InspectionSetups = inspectionSetups;
            Classifications = classifications;
            OtherRelatedDocs = otherRelatedDocs;
            BasicDataText = basicDataText;
        }
    }

    public record MaterialReportCharacteristics
    {
        public string ConsequenceOfFailure { get; }
        public string CreatedBy { get; }
        public string CriticalRating { get; }
        public string DataAdditional { get; }
        public string Division { get; }
        public string InternalDiameter { get; }
        public string LabDesignOffice { get; }
        public string Length { get; }
        public string MirId { get; }
        public string Manufacturer { get; }
        public string ManufacturerPartNumber { get; }
        public string Material { get; }
        public string MaterialGroup { get; }
        public string MatlBasic { get; }
        public string OutsideDiameter { get; }
        public string PolymerSealType { get; }
        public string ProbabilityOfFailure { get; }
        public string SDRLandQM { get; }
        public string StandardMaterial { get; }
        public string TraceabilityCode { get; }
        public string XPlantStatus { get; }

        public MaterialReportCharacteristics(
            string consequenceOfFailure,
            string createdBy,
            string criticalRating,
            string dataAdditional,
            string division,
            string internalDiameter,
            string labDesignOffice,
            string length,
            string mirid,
            string manufacturer,
            string manufacturerPartNumber,
            string material,
            string materialGroup,
            string matlBasic,
            string outsideDiameter,
            string polymerSealType,
            string probabilityOfFailure,
            string sdrLandQM,
            string standardMaterial,
            string traceabilityCode,
            string xPlantStatus)
        {
            ConsequenceOfFailure = consequenceOfFailure;
            CreatedBy = createdBy;
            CriticalRating = criticalRating;
            DataAdditional = dataAdditional;
            Division = division;
            InternalDiameter = internalDiameter;
            LabDesignOffice = labDesignOffice;
            Length = length;
            MirId = mirid;
            Manufacturer = manufacturer;
            ManufacturerPartNumber = manufacturerPartNumber;
            Material = material;
            MaterialGroup = materialGroup;
            MatlBasic = matlBasic;
            OutsideDiameter = outsideDiameter;
            PolymerSealType = polymerSealType;
            ProbabilityOfFailure = probabilityOfFailure;
            SDRLandQM = sdrLandQM;
            StandardMaterial = standardMaterial;
            TraceabilityCode = traceabilityCode;
            XPlantStatus = xPlantStatus;
        }
    }

    public record RelatedDocuments
    {
        public IReadOnlyList<string> Documents { get; }
        public RelatedDocuments(IReadOnlyList<string> documents)
        {
            Documents = documents;
        }
     }
}
