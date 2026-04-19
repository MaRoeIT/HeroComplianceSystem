namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents a Master Document Requirement List document
    /// within the OneSubSea document domain.
    /// </summary>
    public record MasterDocumentRequirementList : Order
    {
        public MasterDocumentRequirementList(
            string orderNumber,
            DateOnly? orderDate
            ) : base(orderNumber, orderDate) { }
    }
}
