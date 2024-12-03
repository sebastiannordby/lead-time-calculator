using LeadTimeCalculator.API.Domain.Shared.Contracts;
using LeadTimeCalculator.API.Domain.Shared.Exceptions;

namespace LeadTimeCalculator.API.Domain.ProductionScheduleFeature
{
    public sealed class Order
    {
        private readonly OrderId _id;
        private readonly Product _product;
        private readonly List<OrderPart> _orderParts;

        public Product Product => _product;
        public OrderId Id => _id;

        public Order(
            OrderId id,
            Product product,
            IEnumerable<OrderPart> orderParts)
        {
            _id = id;
            _product = product;
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
            var productionDuration = _product.ProductionTime;

            var shippingDate = workdayCalendar
                .AddWorkingDays(latestPartArrival, productionDuration.Workdays);

            return shippingDate;
        }
    }
}
