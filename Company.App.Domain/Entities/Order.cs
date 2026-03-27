namespace Company.App.Domain.Entities
{
    public record Order
    {
        public string OrderNumber { get; } = string.Empty;
        public string OrderDate { get; } = string.Empty;
    }
}
