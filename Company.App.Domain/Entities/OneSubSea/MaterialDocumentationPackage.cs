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

        public MaterialDocumentationPackageHeader(
            string documentTitle,
            string purchaseOrderDate,
            string mdpIssueDateTime,
            string documentRevision,
            string ourReference,
            string buyerTelephoneNumber,
            string email)
        {
            DocumentTitle = documentTitle;
            PurchaseOrderDate = purchaseOrderDate;
            MDPIssueDateTime = mdpIssueDateTime;
            DocumentRevision = documentRevision;
            OurReference = ourReference;
            BuyerTelephoneNumber = buyerTelephoneNumber;
            Email = email;
        }
    }

    public record MaterialDocumentationPackageIndex
    {   
        public IReadOnlyList<MaterialMasterReport> MaterialMasterReports { get; }

        public MaterialDocumentationPackageIndex(IReadOnlyList<MaterialMasterReport> materialMasterReports)
        {
            MaterialMasterReports = materialMasterReports;
        }
    }
    public record MaterialMasterReport
    {
        public IReadOnlyList<MaterialReport> MaterialReports { get; }

        public MaterialMasterReport(IReadOnlyList<MaterialReport> materialReports)
        {
            MaterialReports = materialReports;
        }
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
        public IReadOnlyList<string> InspectionSetups { get; }
        public IReadOnlyList<string> Classifications { get; }
        public IReadOnlyList<string> OtherRelatedDocs { get; }
        public IReadOnlyList<string> BasicDataTexts { get; }
        public IReadOnlyList<CharacteristicsItem> Characteristics { get; }
        public BasicDataText MDPBasicDataTexts { get; }

        public MaterialReport(
            string reportDate,
            string materialNumber,
            string description,
            string revisionLevel,
            string plant,
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
            IReadOnlyList<CharacteristicsItem> characteristics,
            BasicDataText mdpBasicDataTexts)
        {
            ReportDate = reportDate;
            MaterialNumber = materialNumber;
            Description = description;
            RevisionLevel = revisionLevel;
            Plant = plant;
            BaseUnitOfMeasure = baseUnitOfMeasure;
            GrossWeight = grossWeight;
            NetWeight = netWeight;
            BasicMaterial = basicMaterial;
            CommodityCode = commodityCode;
            ExportControlCode = exportControlCode;
            Manufacturer = manufacturer;
            ManufacturerPartNumber = manufacturerPartNumber;
            MIRID = mirid;
            MIRTitle = mirTitle;
            MIRDescription = mirDescription;
            Criticality = criticality;
            TraceabilityType = traceabilityType;
            SerialNumberProfile = serialNumberProfile;
            ShelfLife = shelfLife;
            QMControlKey = qmControlKey;
            InspectionSetups = inspectionSetups;
            Classifications = classifications;
            OtherRelatedDocs = otherRelatedDocs;
            BasicDataTexts = basicDataTexts;
            Characteristics = characteristics;
            MDPBasicDataTexts = mdpBasicDataTexts;
        }
    }

    public record CharacteristicsItem
    {
        public string ConsequenceOfFailure { get; }
        public string CreatedBy { get; }
        public string CriticalRating { get; }
        public string DataAdditional { get; }
        public string Division { get; }
        public string InternalDiameter { get; }
        public string LabDesignOffice { get; }
        public string Length { get; }
        public string MIRID { get; }
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

        public CharacteristicsItem(
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
            MIRID = mirid;
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
}
