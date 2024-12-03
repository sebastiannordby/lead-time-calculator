using LeadTimeCalculator.API.Domain.ProductionScheduleFeature;
using LeadTimeCalculator.API.Domain.Shared.Contracts;
using LeadTimeCalculator.API.Domain.Shared.Exceptions;
using NSubstitute;

namespace LeadTimeCalculator.API.Domain.Tests.Unit.Features.ProductionScheduleFeature
{
    public class OrderTests
    {
        [Fact]
        public void CalculateShippingDate_PartsMissingArrivalDateFails()
        {
            // Given
            var workdayCalendarMock = Substitute
                .For<IWorkdayCalendar>();

            var sut = new Order(
                id: new OrderId(1),
                product: new Product(
                    "Robotics Arm",
                    new ProductionTime(1),
                    new ProductType("Robotics")),
                orderParts: [
                    new OrderPart("Bearings", DateTime.Now),
                    new OrderPart("Oil", null),
                    new OrderPart("Silicone", DateTime.Now)
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

            var sut = new Order(
                id: new OrderId(1),
                product: new Product(
                    "Robotics Arm",
                    new ProductionTime(workdaysToProduce),
                    new ProductType("Robotics")),
                orderParts: [
                    new OrderPart("Bearings", lastPartArrivesAt.AddDays(-1)),
                    new OrderPart("Oil", lastPartArrivesAt),
                    new OrderPart("Silicone", lastPartArrivesAt.AddDays(-2))
                ]);

            // When
            var shippingDate = sut
                .CalculateShippingDate(workdayCalendarMock);

            // Then
            Assert.Equal(expectedFinishDate, shippingDate);
        }
    }
}
