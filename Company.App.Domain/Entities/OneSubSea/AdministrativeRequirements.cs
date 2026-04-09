namespace Company.App.Domain.Entities.OneSubSea
{
    /// <summary>
    /// Represents an Administrative Requirements document
    /// within the OneSubSea document domain.
    /// </summary>
    public record AdministrativeRequirements : Order
    {
        public AdministrativeRequirements(
            string orderNumber,
            string orderDate
            ) : base(orderNumber, orderDate) { }
    }
}
