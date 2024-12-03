
using LeadTimeCalculator.API.Domain.Shared.Exceptions;

namespace LeadTimeCalculator.API.Domain.ProductionScheduleFeature
{
    public sealed class ProductionSchedule
    {
        private int _productionCapacity;
        private List<Order> _ordersInSchedule;
        private readonly ProductType _productType;

        public ProductionSchedule(
            int productionCapacity, ProductType productType)
        {
            _productionCapacity = productionCapacity;
            _ordersInSchedule = new();
            _productType = productType;
        }

        public void ScheduleOrder(Order order)
        {
            if (_ordersInSchedule.Count + 1 > _productionCapacity)
                throw new DomainException(
                    "Cannot schedule another order, is will exceed the production capacity.");
            if (order.Product.ProductType != _productType)
                throw new DomainException(
                    $"Cannot schedule order where product type({order.Product.ProductType}) does not match {_productType}");

            _ordersInSchedule.Add(order);
        }
    }
}
