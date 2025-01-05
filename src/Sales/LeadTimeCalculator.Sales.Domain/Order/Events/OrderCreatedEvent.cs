namespace LeadTimeCalculator.Sales.Domain.Order.Events
{
    public record OrderCreatedEvent
    {
        public int OrderNumber { get; init; }
        public string Customer { get; init; }


        public OrderCreatedEvent(
            int orderNumber,
            string customer)
        {
            if (orderNumber <= 0)
                throw new ArgumentException(
                    $"{nameof(orderNumber)} must be greater than 0.");

            if (string.IsNullOrWhiteSpace(customer))
                throw new ArgumentException(
                    $"{nameof(customer)} must be provided.");

            Customer = customer;
        }

        public override string ToString()
        {
            return $"Created order #{OrderNumber} for {Customer}";
        }
    }
}
