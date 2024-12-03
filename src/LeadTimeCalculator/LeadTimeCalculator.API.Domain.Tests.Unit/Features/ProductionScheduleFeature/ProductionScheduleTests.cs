using LeadTimeCalculator.API.Domain.ProductionScheduleFeature;
using LeadTimeCalculator.API.Domain.Shared.Exceptions;

namespace LeadTimeCalculator.API.Domain.Tests.Unit.Features.ProductionScheduleFeature
{
    public class ProductionScheduleTests
    {
        [Fact]
        public void ScheduleOrder_ErrorsWhenExceedsProductionCapacity()
        {
            // Given
            var productType = new ProductType("Robotics");
            var productionSchedule = new ProductionSchedule(
                productionCapacity: 1,
                productType: productType);

            productionSchedule.ScheduleOrder(
                InitializeOrder(productType));

            // When & Then
            var ex = Assert.Throws<DomainException>(() =>
            {
                productionSchedule.ScheduleOrder(
                    InitializeOrder(productType));
            });

            Assert.Contains("exceed",
                ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("production capacity",
                ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ScheduleOrder_ErrorsWhenProductTypeNotMatch()
        {
            // Given
            var productionSchedule = new ProductionSchedule(
                productionCapacity: 1,
                productType: new ProductType("Robotics"));

            // When & Then
            var ex = Assert.Throws<DomainException>(() =>
            {
                productionSchedule.ScheduleOrder(
                    InitializeOrder(new ProductType("Liquids")));
            });

            Assert.Contains("product type",
                ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("not match",
                ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        private static Order InitializeOrder(
            ProductType productType)
        {
            return new Order(
                id: new OrderId(1),
                product: new Product(
                    "Robotics Leg",
                    new ProductionTime(1),
                    productType),
                orderParts: [
                    new OrderPart("Silicone", DateTime.Now)
                ]);
        }
    }
}
