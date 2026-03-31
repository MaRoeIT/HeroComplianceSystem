namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents a purchase order document with header information,
    /// overhead sections, line items, and total values.
    /// </summary>
    public record PurchaseOrder : Order
    {
        // Gets the structured header information for the purchase order.
        public PurchaseOrderHeader Header { get; } = new();

        // Gets the overhead section content of the purchase order,
        // such as general document sections and free-text blocks.
        public PurchaseOrderOverhead Overhead { get; } = new();

        // Gets the collection of line items included in the purchase order.
        public List<PurchaseOrderItem> Items { get; } = new();

        // Gets the total net value for the purchase order.
        public string TotalNetValue { get; } = string.Empty;

        // Gets the total quantity or amount summary for the purchase order.
        public string TotalAmount { get; } = string.Empty;

    }
    
    /// <summary>
    /// Represents the header section of a purchase order,
    /// including buyer, seller, and address-related information.
    /// </summary>
    public record PurchaseOrderHeader
    {
        // Gets the seller-related details found in the purchase order header.
        public Seller Seller { get; } = new();

        // Gets the buyer-related details found in the purchase order header.
        public Buyer Buyer { get; } = new();

        // Gets the vendor address associated with the order.
        public Address VendorAddress { get; } = new();

        // Gets the invoice address associated with the order.
        public Address InvoiceAddress { get; } = new();

        // Gets the delivery address associated with the order.
        public DeliveryAddress DeliveryAddress { get; } = new();
    }

    /// <summary>
    /// Represents seller-specific information extracted from a purchase order.
    /// </summary>
    public record Seller
    {
        // Gets the purchase order number referenced by the seller section.
        public string PurchaseOrderNumber { get; } = string.Empty;

        // Gets the vendor number.
        public string VendorNumber { get; } = string.Empty;

        // Gets the fax number for the seller.
        public string FaxNumber { get; } = string.Empty;

        // Gets the email address for the seller.
        public string Email { get; } = string.Empty;

        // Gets the contact person for the seller.
        public string ContactPerson { get; } = string.Empty;

        // Gets the frame agreement reference.
        public string FrameAgreement { get; } = string.Empty;

        // Gets the Incoterms code or label.
        public string IncoTerms { get; } = string.Empty;

        // Gets the descriptive text related to the Incoterms field.
        public string IncoTermsDescription { get; } = string.Empty;

        // Gets the major or minor classification value.
        public string MajorMinor { get; } = string.Empty;

        // Gets the intercompany indicator or value.
        public string Intercompany { get; } = string.Empty;
    }

    /// <summary>
    /// Represents buyer-specific information extracted from a purchase order.
    /// </summary>
    public record Buyer
    {
        // Gets the revision number of the purchase order.
        public string RevisionNumber { get; } = string.Empty;

        // Gets the creation date of the purchase order.
        public string DateCreated { get; } = string.Empty;

        // Gets the currency used in the purchase order.
        public string Currency { get; } = string.Empty;

        // Gets the buyer contact person.
        public string ContactPerson { get; } = string.Empty;

        // Gets the confirmation fax value.
        public string ConfirmationFax { get; } = string.Empty;

        // Gets the frame agreement reference.
        public string FrameAgreement { get; } = string.Empty;

        // Gets the payment terms for the order.
        public string PaymentTerms { get; } = string.Empty;

        // Gets the technical contact reference.
        public string TechnicalContact { get; } = string.Empty;

        // Gets the quality-system responsible contact or reference.
        public string QSResponsible { get; } = string.Empty;
    }

    /// <summary>
    /// Represents a general postal address used in the purchase order.
    /// </summary>
    public record Address
    {
        // Gets the company name for the address.
        public string CompanyName { get; } = string.Empty;

        // Gets the street name for the address.
        public string StreetName { get; } = string.Empty;

        // Gets the street number for the address.
        public string StreetNumber { get; } = string.Empty;

        // Gets the postal code for the address.
        public string PostalNumber { get; } = string.Empty;

        // Gets the city for the address.
        public string City { get; } = string.Empty;

        // Gets the country for the address.
        public string? Country { get; } = string.Empty;
    }

    /// <summary>
    /// Represents a delivery address with an additional customer type field.
    /// </summary>
    public record DeliveryAddress : Address
    {
        // Gets the customer type associated with the delivery address.
        public string CustomerType { get; } = string.Empty;
    }

    /// <summary>
    /// Represents a structured overhead section in the purchase order,
    /// including nested subsections and bullet-point content.
    /// </summary>
    public record PurchaseOrderOverhead
    {
        // Gets the page numbers where this section appears.
        public List<int> PageNumber { get; } = new();

        // Gets the logical section identifier or label.
        public string Section { get; } = string.Empty;

        // Gets the title of the section.
        public string Title { get; } = string.Empty;

        // Gets the free-text content lines belonging to this section.
        public List<string> Content { get; } = new();

        // Gets the bullet-point entries belonging to this section.
        public List<string> BulletPoints { get; } = new();

        // Gets nested subsections contained within this section.
        public List<PurchaseOrderOverhead> SubSections { get; } = new();
    }

    /// <summary>
    /// Represents one line item in the purchase order.
    /// </summary>
    public record PurchaseOrderItem
    {
        // Gets the line item number.
        public string ItemNumber { get; } = string.Empty;

        // Gets the page numbers where this item appears.
        public List<int> PageNumber { get; } = new();

        // Gets the material descriptions associated with this item.
        public List<MaterialDescription> MaterialDescriptions { get; } = new();

        // Gets the revision value for the item.
        public string Revision { get; } = string.Empty;

        // Gets the ordered quantity.
        public string Quantity { get; } = string.Empty;

        // Gets the order unit.
        public string OrderUnit { get; } = string.Empty;

        // Gets the net price for the item.
        public string NetPrice { get; } = string.Empty;

        // Gets the net amount for the item.
        public string NetAmount { get; } = string.Empty;
    }

    /// <summary>
    /// Represents a material description block associated with a purchase order item.
    /// </summary>
    public record MaterialDescription
    {
        // Gets the material number.
        public string MaterialNumber { get; } = string.Empty;

        // Gets the material name or description.
        public string MaterialName { get; } = string.Empty;

        // Gets the traceability code.
        public string TraceabilityCode { get; } = string.Empty;

        // Gets the reference to the supplier traceability specification document.
        public string DocRefTraceabilitySpecificationForSuppliers { get; } = string.Empty;

        // Gets the outline agreement number.
        public string OutlineAgreementNumber { get; } = string.Empty;

        // Gets related basic data text values for the material.
        public BasicDataText BasicDataTexts { get; } = new();

        // Gets the manufacturer name.
        public string Manufacturer { get; } = string.Empty;

        // Gets the work breakdown structure reference.
        public string WorkBreakdownStructure { get; } = string.Empty;

        // Gets the manufacturer part number.
        public string ManufacturerPartNo { get; } = string.Empty;

        // Gets the delivery date for the material, if available.
        public string? DeliveryDate { get; }
    }

    /// <summary>
    /// Represents additional basic data text values associated with a material description.
    /// </summary>
    public record BasicDataText
    {
        // Gets the delivery requirement expiry date text.
        public string DeliveryRequirementExpiryDate { get; } = string.Empty;

        // Gets the Seal SQ name.
        public string SealSQName { get; } = string.Empty;

        // Gets the Seal SQ description.
        public string SealSQDescription { get; } = string.Empty;

        // Gets the Seal Engineering part number.
        public string SealEngineeringPartNumber { get; } = string.Empty;

        // Gets the T-seal value.
        public string TSeal { get; } = string.Empty;

        // Gets the anti-extrusion rings value.
        public string AntiExtrusionRings { get; } = string.Empty;
    }
}
