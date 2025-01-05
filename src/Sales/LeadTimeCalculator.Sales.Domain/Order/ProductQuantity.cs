namespace LeadTimeCalculator.Sales.Domain.Order
{
    public class ProductQuantity
    {
        public QuantityType Type { get; }
        public double Quantity { get; }

        public ProductQuantity(QuantityType type, double quantity)
        {
            if (type == QuantityType.Whole && quantity % 1 != 0)
            {
                throw new ArgumentException("Quantity must be a whole number for QuantityType.Whole.", nameof(quantity));
            }

            if (type == QuantityType.Partial && quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero for QuantityType.Partial.", nameof(quantity));
            }

            Type = type;
            Quantity = quantity;
        }

        public enum QuantityType
        {
            Whole = 0,
            Partial = 1
        }
    }
}
