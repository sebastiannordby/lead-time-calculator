using LeadTimeCalculator.Production.Domain.Exceptions;
using LeadTimeCalculator.Production.Domain.Models.Calendar;
using LeadTimeCalculator.Production.Domain.Models.Order;
using LeadTimeCalculator.Production.Domain.Models.Schedule;

namespace LeadTimeCalculator.Production.Domain.Tests.Unit.Schedule
{
    public class OrderTests
    {
        [Fact]
        public void CalculateShippingDate_PartsMissingArrivalDateFails()
        {
            // Given
            var workdayCalendarMock = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                new WorkWeek(new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16))),
                holidays: Enumerable.Empty<Holiday>());

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
            var lastPartArrivesAt = DateTime.Parse("2024-12-13 08:00");
            var workdaysToProduce = 5;

            var workdayCalendar = new WorkdayCalendar(
                defaultWorkhoursPerDay: 8,
                new WorkWeek(new WorkHours(TimeSpan.FromHours(8), TimeSpan.FromHours(16))),
                holidays: Enumerable.Empty<Holiday>());

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
                .CalculateShippingDate(workdayCalendar);

            // Then
            var expectedShippingDate = DateTime.Parse("2024-12-19 16:00");
            Assert.Equal(expectedShippingDate, shippingDate);
        }
    }
}
