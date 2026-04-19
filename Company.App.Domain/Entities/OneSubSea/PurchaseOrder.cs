namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents a purchase order document with header information,
    /// overhead sections, line items, and total values.
    /// </summary>
    public record PurchaseOrder : Order
    {
        // Gets the structured header information for the purchase order.
        public PurchaseOrderHeader Header { get; }

        // Gets the overhead section content of the purchase order,
        // such as general document sections and free-text blocks.
        public PurchaseOrderOverhead Overhead { get; }

        // Gets the collection of line items included in the purchase order.
        public IReadOnlyList<PurchaseOrderItem> Items { get; }

        // Gets the total net value for the purchase order.
        public string TotalNetValue { get; }

        // Gets the total quantity or amount summary for the purchase order.
        public string TotalAmount { get; }

        // Constructor.
        public PurchaseOrder(
            string orderNumber,
            DateOnly? orderDate,
            PurchaseOrderHeader header,
            PurchaseOrderOverhead overhead,
            IReadOnlyList<PurchaseOrderItem> items,
            string totalNetValue,
            string totalAmount
        ) : base(orderNumber, orderDate)
        {
            Header = header;
            Overhead = overhead;
            Items = items;
            TotalNetValue = totalNetValue;
            TotalAmount = totalAmount;
        }
    }

    /// <summary>
    /// Represents the header section of a purchase order,
    /// including buyer, seller, and address-related information.
    /// </summary>
    public record PurchaseOrderHeader
    {
        // Gets the seller-related details found in the purchase order header.
        public Seller Seller { get; }

        // Gets the buyer-related details found in the purchase order header.
        public Buyer Buyer { get; }

        // Gets the vendor address associated with the order.
        public Address VendorAddress { get; }

        // Gets the invoice address associated with the order.
        public Address InvoiceAddress { get; }

        // Gets the delivery address associated with the order.
        public DeliveryAddress DeliveryAddress { get; }

        // Constructor.
        public PurchaseOrderHeader(
            Seller seller,
            Buyer buyer,
            Address vendorAddress,
            Address invoiceAddress,
            DeliveryAddress deliveryAddress)
        {
            Seller = seller;
            Buyer = buyer;
            VendorAddress = vendorAddress;
            InvoiceAddress = invoiceAddress;
            DeliveryAddress = deliveryAddress;
        }
    }

    /// <summary>
    /// Represents seller-specific information extracted from a purchase order.
    /// </summary>
    public record Seller
    {
        // Gets the purchase order number referenced by the seller section.
        public string PurchaseOrderNumber { get; }

        // Gets the vendor number.
        public string VendorNumber { get; }

        // Gets the fax number for the seller.
        public string FaxNumber { get; }

        // Gets the email address for the seller.
        public string Email { get; }

        // Gets the contact person for the seller.
        public string ContactPerson { get; }

        // Gets the frame agreement reference.
        public string FrameAgreement { get; }

        // Gets the Incoterms code or label.
        public string IncoTerms { get; }

        // Gets the descriptive text related to the Incoterms field.
        public string IncoTermsDescription { get; }

        // Gets the major or minor classification value.
        public string MajorMinor { get; }

        // Gets the intercompany indicator or value.
        public string Intercompany { get; }

        // Constructor.
        public Seller(
            string purchaseOrderNumber,
            string vendorNumber,
            string faxNumber,
            string email,
            string contactPerson,
            string frameAgreement,
            string incoTerms,
            string incoTermsDescription,
            string majorMinor,
            string intercompany)
        {
            PurchaseOrderNumber = purchaseOrderNumber;
            VendorNumber = vendorNumber;
            FaxNumber = faxNumber;
            Email = email;
            ContactPerson = contactPerson;
            FrameAgreement = frameAgreement;
            IncoTerms = incoTerms;
            IncoTermsDescription = incoTermsDescription;
            MajorMinor = majorMinor;
            Intercompany = intercompany;
        }
    }

    /// <summary>
    /// Represents buyer-specific information extracted from a purchase order.
    /// </summary>
    public record Buyer
    {
        // Gets the revision number of the purchase order.
        public string RevisionNumber { get; }

        // Gets the creation date of the purchase order.
        public string DateCreated { get; }

        // Gets the currency used in the purchase order.
        public string Currency { get; }

        // Gets the buyer contact person.
        public string ContactPerson { get; }

        // Gets the confirmation fax value.
        public string ConfirmationFax { get; }

        // Gets the frame agreement reference.
        public string FrameAgreement { get; }

        // Gets the payment terms for the order.
        public string PaymentTerms { get; }

        // Gets the technical contact reference.
        public string TechnicalContact { get; }

        // Gets the quality-system responsible contact or reference.
        public string QSResponsible { get; }

        // Constructor.
        public Buyer(
            string revisionNumber,
            string dateCreated,
            string currency,
            string contactPerson,
            string confirmationFax,
            string frameAgreement,
            string paymentTerms,
            string technicalContact,
            string qSResponsible)
        {
            RevisionNumber = revisionNumber;
            DateCreated = dateCreated;
            Currency = currency;
            ContactPerson = contactPerson;
            ConfirmationFax = confirmationFax;
            FrameAgreement = frameAgreement;
            PaymentTerms = paymentTerms;
            TechnicalContact = technicalContact;
            QSResponsible = qSResponsible;
        }
    }

    /// <summary>
    /// Represents a general postal address used in the purchase order.
    /// </summary>
    public record Address
    {
        // Gets the company name for the address.
        public string CompanyName { get; }

        // Gets the street name for the address.
        public string StreetName { get; }

        // Gets the street number for the address.
        public string StreetNumber { get; }

        // Gets the postal code for the address.
        public string PostalNumber { get; }

        // Gets the city for the address.
        public string City { get; }

        // Gets the country for the address.
        public string? Country { get; }

        // Constructor.
        public Address(
            string companyName,
            string streetName,
            string streetNumber,
            string postalNumber,
            string city,
            string? country)
        {
            CompanyName = companyName;
            StreetName = streetName;
            StreetNumber = streetNumber;
            PostalNumber = postalNumber;
            City = city;
            Country = country;
        }
    }

    /// <summary>
    /// Represents a delivery address with an additional customer type field.
    /// </summary>
    public record DeliveryAddress : Address
    {
        // Gets the customer type associated with the delivery address.
        public string CustomerType { get; }

        // Constructor.
        public DeliveryAddress(
            string companyName,
            string streetName,
            string streetNumber,
            string postalNumber,
            string city,
            string? country,
            string customerType
        ) : base(
                companyName,
                streetName,
                streetNumber,
                postalNumber,
                city,
                country)
        {
            CustomerType = customerType;
        }
    }

    /// <summary>
    /// Represents a structured overhead section in the purchase order,
    /// including nested subsections and bullet-point content.
    /// </summary>
    public record PurchaseOrderOverhead
    {
        // Gets the page numbers where this section appears.
        public HashSet<int> PageNumbers { get; }

        // Gets the logical section identifier or label.
        public string SectionNumber { get; }

        // Gets the title of the section.
        public string Title { get; }

        // Gets the free-text content lines belonging to this section.
        public IReadOnlyList<string> Content { get; }

        // Gets nested subsections contained within this section.
        public IReadOnlyList<PurchaseOrderOverhead> SubSections { get; }

        // Constructor.
        public PurchaseOrderOverhead(
            HashSet<int> pageNumbers,
            string sectionNumber,
            string title,
            IReadOnlyList<string> content,
            IReadOnlyList<PurchaseOrderOverhead> subSections)
        {
            PageNumbers = pageNumbers;
            SectionNumber = sectionNumber ?? "";
            Title = title;
            Content = content ?? Array.Empty<string>();
            SubSections = subSections ?? Array.Empty<PurchaseOrderOverhead>();
        }
    }

    /// <summary>
    /// Represents one line item in the purchase order.
    /// </summary>
    public record PurchaseOrderItem
    {
        // Gets the line item number.
        public string ItemNumber { get; }

        // Gets the page numbers where this item appears.
        public HashSet<int> PageNumber { get; }

        // Gets the material descriptions associated with this item.
        public MaterialDescription MaterialDescriptions { get; }

        // Gets the revision value for the item.
        public string Revision { get; }

        // Gets the ordered quantity.
        public string Quantity { get; }

        // Gets the Unit order quantity.
        public string Order { get; }
        
        // Gets the Unit Type
        public string Unit { get; }

        // Gets the net price for the item.
        public string NetPrice { get; }

        // Gets the net amount for the item.
        public string NetAmount { get; }

        // Constructor.
        public PurchaseOrderItem(
            string itemNumber,
            HashSet<int> pageNumber,
            MaterialDescription materialDescriptions,
            string revision,
            string quantity,
            string order,
            string unit,
            string netPrice,
            string netAmount)
        {
            ItemNumber = itemNumber;
            PageNumber = pageNumber;
            MaterialDescriptions = materialDescriptions;
            Revision = revision;
            Quantity = quantity;
            Order = order;
            Unit = unit;
            NetPrice = netPrice;
            NetAmount = netAmount;
        }
    }

    /// <summary>
    /// Represents a material description block associated with a purchase order item.
    /// </summary>
    public record MaterialDescription
    {
        // Gets the material number.
        public string MaterialNumber { get; }

        // Gets the material name or description.
        public string MaterialName { get; }

        // Gets the traceability code.
        public string TraceabilityCode { get; }

        // Gets the reference to the supplier traceability specification document.
        public string DocRefTraceabilitySpecificationForSuppliers { get; }

        // Gets the outline agreement number.
        public string OutlineAgreementNumber { get; }

        // Gets related basic data text values for the material.
        public BasicDataText BasicDataTexts { get; }

        // Gets the manufacturer name.
        public string Manufacturer { get; }

        // Gets the work breakdown structure reference.
        public string WorkBreakdownStructure { get; }

        // Gets the manufacturer part number.
        public string ManufacturerPartNo { get; }

        // Gets the delivery date for the material, if available.
        public string? DeliveryDate { get; }

        // Constructor.
        public MaterialDescription(
            string materialNumber,
            string materialName,
            string traceabilityCode,
            string docRefTraceabilitySpecificationForSuppliers,
            string outlineAgreementNumber,
            BasicDataText basicDataTexts,
            string manufacturer,
            string workBreakdownStructure,
            string manufacturerPartNo,
            string? deliveryDate)
        {
            MaterialNumber = materialNumber;
            MaterialName = materialName;
            TraceabilityCode = traceabilityCode;
            DocRefTraceabilitySpecificationForSuppliers = docRefTraceabilitySpecificationForSuppliers;
            OutlineAgreementNumber = outlineAgreementNumber;
            BasicDataTexts = basicDataTexts;
            Manufacturer = manufacturer;
            WorkBreakdownStructure = workBreakdownStructure;
            ManufacturerPartNo = manufacturerPartNo;
            DeliveryDate = deliveryDate;
        }
    }

}
