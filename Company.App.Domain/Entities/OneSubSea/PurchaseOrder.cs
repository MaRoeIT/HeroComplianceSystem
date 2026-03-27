namespace Company.App.Domain.Entities.OneSubSea
{
    public record PurchaseOrder : Order
    {
        public PurchaseOrderHeader Header { get; } = new();
        public PurchaseOrderOverhead Overhead { get; } = new();
        public List<PurchaseOrderItem> Items { get; } = new();
        public string TotalNetValue { get; } = string.Empty;
        public string TotalAmount { get; } = string.Empty;

    }
    public record PurchaseOrderHeader
    {
        public Seller Seller { get; } = new();
        public Buyer Buyer { get; } = new();

        public Address VendorAddress { get; } = new();
        public Address InvoiceAddress { get; } = new();
        public DeliveryAddress DeliveryAddress { get; } = new();
    }
    public record Seller
    {
        public string PurchaseOrderNumber { get; } = string.Empty;
        public string VendorNumber { get; } = string.Empty;
        public string FaxNumber { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public string ContactPerson { get; } = string.Empty;
        public string FrameAgreement { get; } = string.Empty;
        public string IncoTerms { get; } = string.Empty;
        public string IncoTermsDescription { get; } = string.Empty;
        public string MajorMinor { get; } = string.Empty;
        public string Intercompany { get; } = string.Empty;
    }
    public record Buyer
    {
        public string RevisionNumber { get; } = string.Empty;
        public string DateCreated { get; } = string.Empty;
        public string Currency { get; } = string.Empty;
        public string ContactPerson { get; } = string.Empty;
        public string ConfirmatioFax { get; } = string.Empty;
        public string FrameAgreement { get; } = string.Empty;
        public string PaymentTerms { get; } = string.Empty;
        public string TechnicalContact { get; } = string.Empty;
        public string QSResponsible { get; } = string.Empty;
    }
    public record Address
    {
        public string CompanyName { get; } = string.Empty;
        public string StreetName { get; } = string.Empty;
        public string StreetNumber { get; } = string.Empty;
        public string PostalNumber { get; } = string.Empty;
        public string City { get; } = string.Empty;
        public string? Country { get; } = string.Empty;
    }
    public record DeliveryAddress : Address
    {
        public string CustomerType { get; } = string.Empty;
    }
    public record PurchaseOrderOverhead
    {
        public List<int> PageNumber { get; } = new();
        public string Section { get; } = string.Empty;
        public string Title { get; } = string.Empty;
        public List<string> Content { get; } = new();
        public List<string> BulletPoints { get; } = new();
        public List<PurchaseOrderOverhead> SubSections { get; } = new();
    }
    public record PurchaseOrderItem
    {
        public string ItemNumber { get; } = string.Empty;
        public List<int> PageNumber { get; } = new();
        public List<MaterialDescription> MaterialDescriptions { get; } = new();
        public string Revision { get; } = string.Empty;
        public string Quantity { get; } = string.Empty;
        public string OrderUnit { get; } = string.Empty;
        public string NetPrice { get; } = string.Empty;
        public string NetAmount { get; } = string.Empty;
    }

    public record MaterialDescription
    {
        public string MaterialNumber { get; } = string.Empty;
        public string MaterialName { get; } = string.Empty;
        public string TracebillityCode { get; } = string.Empty;
        public string DocRefTraceabilitySpecificationForSuppliers { get; } = string.Empty;
        public string OutlineAgreementNumber { get; } = string.Empty;
        public BasicDataText BasicDataTexts { get; } = new();
        public string Manufacturer { get; } = string.Empty;
        public string WorkBreakdownStructure { get; } = string.Empty;
        public string ManufacturerPartNo { get; } = string.Empty;
        public DateTime? DeliveryDate { get; }
    }

    public record BasicDataText
    {
        public string DeliveryRequirementExpiryDate { get; } = string.Empty;
        public string SealSQName { get; } = string.Empty;
        public string SealSQDescription { get; } = string.Empty;
        public string SealEngineeringPartNumber { get; } = string.Empty;
        public string TSeal { get; } = string.Empty;
        public string AntiExtrutionRings { get; } = string.Empty;
    }
}
