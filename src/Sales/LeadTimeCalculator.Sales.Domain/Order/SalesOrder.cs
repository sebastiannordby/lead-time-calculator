using LeadTimeCalculator.Sales.Domain.Order.Events;

namespace LeadTimeCalculator.Sales.Domain.Order
{
    public class SalesOrder
    {
        public string Customer { get; private set; } = null!;
        public List<SalesOrderLine> OrderLines { get; set; } = new();

        public IReadOnlyCollection<object> UncommittedEvents => _uncommittedEvents.AsReadOnly();
        private readonly List<object> _uncommittedEvents = new();

        public SalesOrder(IEnumerable<object> eventStream)
        {
            foreach (var @event in eventStream)
            {
                Apply(@event);
            }
        }

        public static SalesOrder CreateOrder(
            int orderNumber, string customer)
        {
            var order = new SalesOrder(
                Enumerable.Empty<object>());
            order.ApplyChange(new OrderCreatedEvent(orderNumber, customer));

            return order;
        }

        public void AddProduct(
            ProductDetails details)
        {
            ApplyChange(new AddProductEvent(
                details.ProductId,
                details.ProductName,
                details.Quantity,
                details.Price));
        }

        private void ApplyChange(object @event)
        {
            Apply(@event);
            _uncommittedEvents.Add(@event);
        }

        private void Apply(object @event)
        {
            if (@event is null)
                throw new ArgumentNullException("Cannot apply event of null.");

            switch (@event)
            {
                case OrderCreatedEvent e:
                    Customer = e.Customer;
                    break;

                case AddProductEvent e:
                    OrderLines.Add(new SalesOrderLine(
                        e.ProductId,
                        e.ProductName,
                        e.Quantity,
                        e.Price));
                    break;

                default:
                    throw new ArgumentException(
                        $"Not supported event {@event.GetType().Name}");
            }
        }
    }
}
