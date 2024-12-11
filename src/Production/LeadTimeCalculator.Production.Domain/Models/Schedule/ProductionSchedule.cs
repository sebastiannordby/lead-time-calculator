using LeadTimeCalculator.Production.Domain.Exceptions;
using LeadTimeCalculator.Production.Domain.Models.Order;

namespace LeadTimeCalculator.Production.Domain.Models.Schedule
{
    public sealed class ProductionSchedule
    {
        public int ProductionCapacity { get; }
        public ProductType ProductType { get; }
        public IReadOnlyCollection<ProductionOrder> ScheduledOrders => _scheduledOrders;

        private List<ProductionOrder> _scheduledOrders;

        public ProductionSchedule(
            int productionCapacity,
            ProductType productType)
        {
            ProductionCapacity = productionCapacity;
            _scheduledOrders = new();
            ProductType = productType;
        }

        public void ScheduleOrder(ProductionOrder order)
        {
            if (_scheduledOrders.Count + 1 > ProductionCapacity)
                throw new DomainException(
                    "Cannot schedule another order, is will exceed the production capacity.");
            if (order.Product.ProductType != ProductType)
                throw new DomainException(
                    $"Cannot schedule order where product type({order.Product.ProductType}) does not match {ProductType}");

            _scheduledOrders.Add(order);
        }
    }
}
