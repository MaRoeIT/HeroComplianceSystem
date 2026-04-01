namespace Company.App.Domain.Entities
{
    /// <summary>
    /// Represents the common base data shared by supported order-related documents.
    /// </summary>
    public record Order
    {
        // Gets the order number associated with the document.
        public string OrderNumber { get; }

        // Gets the order date associated with the document.
        public string OrderDate { get; }

        // Contructor
        public Order(string orderNumber, string orderDate)
        {
            OrderNumber = orderNumber;
            OrderDate = orderDate;
        }
    }
}
