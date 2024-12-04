using LeadTimeCalculator.API.Domain.Shared.Contracts;
using LeadTimeCalculator.API.Domain.Shared.Exceptions;

namespace LeadTimeCalculator.API.Domain.ProductionScheduleFeature.Models
{
    public sealed class ProductionOrder
    {
        public ProductionOrderId Id { get; }
        public ProducableProduct Product { get; }
        public IReadOnlyCollection<ProductionOrderPart> OrderParts => _orderParts;

        public List<ProductionOrderPart> _orderParts;

        public ProductionOrder(
            ProductionOrderId id,
            ProducableProduct product,
            IEnumerable<ProductionOrderPart> orderParts)
        {
            Id = id;
            Product = product;
            _orderParts = orderParts.ToList();
        }

        public DateTime CalculateShippingDate(
            IWorkdayCalendar workdayCalendar)
        {
            if (_orderParts.Any(part => !part.ExpectedArrivalDate.HasValue))
                throw new DomainException(
                    "Cannot calculate shipping date when not all parts has confirmed arrival date");

            var latestPartArrival = _orderParts
                .Max(part => part.ExpectedArrivalDate!.Value);
            var productionDuration = Product.ProductionTime;

            var shippingDate = workdayCalendar
                .AddWorkingDays(latestPartArrival, productionDuration.Workdays);

            return shippingDate;
        }
    }
}
