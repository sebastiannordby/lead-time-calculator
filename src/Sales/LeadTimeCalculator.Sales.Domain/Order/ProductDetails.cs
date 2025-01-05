namespace LeadTimeCalculator.Sales.Domain.Order
{
    public record ProductDetails
    {
        public Guid ProductId { get; }
        public string ProductName { get; }
        public ProductQuantity Quantity { get; }
        public double Price { get; }

        public ProductDetails(
            Guid productId,
            string productName,
            ProductQuantity quantity,
            double price)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}
