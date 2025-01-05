namespace LeadTimeCalculator.Sales.Domain.Order.Events
{
    public class AddProductEvent
    {
        public Guid ProductId { get; }
        public string ProductName { get; }
        public ProductQuantity Quantity { get; }
        public double Price { get; }

        public AddProductEvent(
            Guid productId,
            string productName,
            ProductQuantity quantity,
            double price)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException(nameof(productId));
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException(nameof(productName));
            if (quantity is null)
                throw new ArgumentException(nameof(quantity));

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}
