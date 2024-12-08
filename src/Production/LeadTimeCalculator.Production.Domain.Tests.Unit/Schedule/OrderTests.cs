using LeadTimeCalculator.Production.Domain.Models.Order;
using LeadTimeCalculator.Production.Domain.Models.Schedule;
using LeadTimeCalculator.Production.Domain.Shared.Contracts;
using LeadTimeCalculator.Production.Domain.Shared.Exceptions;
using NSubstitute;

namespace LeadTimeCalculator.Production.Domain.Tests.Unit.Schedule
{
    public class OrderTests
    {
        [Fact]
        public void CalculateShippingDate_PartsMissingArrivalDateFails()
        {
            // Given
            var workdayCalendarMock = Substitute
                .For<IWorkdayCalendar>();

            var sut = new ProductionOrder(
                id: new ProductionOrderId(1),
                product: new ProducableProduct(
                    "Robotics Arm",
                    new ProductionTime(1),
                    new ProductType("Robotics")),
                orderParts: [
                    new ProductionOrderPart("Bearings", DateTime.Now),
                    new ProductionOrderPart("Oil", null),
                    new ProductionOrderPart("Silicone", DateTime.Now)
                ]);

            // When & Then
            var ex = Assert.Throws<DomainException>(() =>
            {
                sut.CalculateShippingDate(workdayCalendarMock);
            });

            Assert.Contains("arrival date",
                ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CalculateShippingDate_ShouldUseLatestArrivalDateForCalculation()
        {
            // Given
            var timeNow = DateTime.Now;
            var lastPartArrivesAt = timeNow.AddDays(10);
            var workdaysToProduce = 5;
            var expectedFinishDate = lastPartArrivesAt.AddDays(5);

            var workdayCalendarMock = Substitute
                .For<IWorkdayCalendar>();

            workdayCalendarMock
                .AddWorkingDays(timeNow.AddDays(10), workdaysToProduce)
                .Returns(lastPartArrivesAt.AddDays(workdaysToProduce));

            var sut = new ProductionOrder(
                id: new ProductionOrderId(1),
                product: new ProducableProduct(
                    "Robotics Arm",
                    new ProductionTime(workdaysToProduce),
                    new ProductType("Robotics")),
                orderParts: [
                    new ProductionOrderPart("Bearings", lastPartArrivesAt.AddDays(-1)),
                    new ProductionOrderPart("Oil", lastPartArrivesAt),
                    new ProductionOrderPart("Silicone", lastPartArrivesAt.AddDays(-2))
                ]);

            // When
            var shippingDate = sut
                .CalculateShippingDate(workdayCalendarMock);

            // Then
            Assert.Equal(expectedFinishDate, shippingDate);
        }
    }
}
