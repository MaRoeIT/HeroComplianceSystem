using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Domain.Entities
{
    public record PurchaseOrder
    {
        public string OrderNumber { get; } = string.Empty;
        public string OrderDate { get; } = string.Empty;
        public PurchaseOrderHeader Header { get; } = new();
        public PurchaseOrderOverhead Overhead { get; } = new();
        public List<PurchaseOrderItem> Items { get; } = new();
        public string TotalNetValue { get; } = string.Empty;
        public string TotalAmount { get; } = string.Empty;

    }
    public class PurchaseOrderHeader
    {
        public Seller Seller { get; set; } = new();
        public Buyer Buyer { get; set; } = new();

        public Address VendorAddress { get; set; } = new();
        public Address InvoiceAddress { get; set; } = new();
        public DeliveryAddress DeliveryAddress { get; set; } = new();
    }
    public class Seller
    {
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public string VendorNumber { get; set; } = string.Empty;
        public string FaxNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string FrameAgreement { get; set; } = string.Empty;
        public string IncoTerms { get; set; } = string.Empty;
        public string IncoTermsDescription { get; set; } = string.Empty;
        public string MajorMinor { get; set; } = string.Empty;
        public string Intercompany { get; set; } = string.Empty;
    }
    public class Buyer
    {
        public string RevisionNumber { get; set; } = string.Empty;
        public string DateCreated { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ConfirmatioFax { get; set; } = string.Empty;
        public string FrameAgreement { get; set; } = string.Empty;
        public string PaymentTerms { get; set; } = string.Empty;
        public string TechnicalContact { get; set; } = string.Empty;
        public string QSResponsible { get; set; } = string.Empty;
    }
    public class Address
    {
        public string CompanyName { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
        public string StreetNumber { get; set; } = string.Empty;
        public string PostalNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
    }
    public class DeliveryAddress : Address
    {
        public string CustomerType { get; set; } = string.Empty;
    }
    public class PurchaseOrderOverhead
    {
        public List<int> PageNumber { get; set; } = new();
        public string Section { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Content { get; set; } = new();
        public List<string> BulletPoints { get; set; } = new();
        public List<PurchaseOrderOverhead> SubSections { get; set; } = new();
    }
    public class PurchaseOrderItem
    {
        public string ItemNumber { get; set; } = string.Empty;
        public List<int> PageNumber { get; set; } = new();
        public List<MaterialDescription> MaterialDescriptions { get; set; } = new();
        public string Revision { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public string OrderUnit { get; set; } = string.Empty;
        public string NetPrice { get; set; } = string.Empty;
        public string NetAmount { get; set; } = string.Empty;
    }

    public class MaterialDescription
    {
        public string MaterialNumber { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public string TracebillityCode { get; set; } = string.Empty;
        public string DocRefTraceabilitySpecificationForSuppliers { get; set; } = string.Empty;
        public string OutlineAgreementNumber { get; set; } = string.Empty;
        public BasicDataText BasicDataTexts { get; set; } = new();
        public string Manufacturer { get; set; } = string.Empty;
        public string WorkBreakdownStructure { get; set; } = string.Empty;
        public string ManufacturerPartNo { get; set; } = string.Empty;
        public DateTime? DeliveryDate { get; set; }
    }

    public class BasicDataText
    {
        public string DeliveryRequirementExpiryDate { get; set; } = string.Empty;
        public string SealSQName { get; set; } = string.Empty;
        public string SealSQDescription { get; set; } = string.Empty;
        public string SealEngineeringPartNumber { get; set; } = string.Empty;
        public string TSeal { get; set; } = string.Empty;
        public string AntiExtrutionRings { get; set; } = string.Empty;
    }
}
