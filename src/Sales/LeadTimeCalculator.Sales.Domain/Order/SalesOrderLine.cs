
namespace LeadTimeCalculator.Sales.Domain.Order
{
    public class SalesOrderLine
    {
        public Guid ProductId { get; }
        public string ProductName { get; }
        public ProductQuantity Quantity { get; }
        public double Price { get; }

        public SalesOrderLine(
            Guid productId,
            string productName,
            ProductQuantity quantity,
            double productPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = productPrice;
        }
    }
}
