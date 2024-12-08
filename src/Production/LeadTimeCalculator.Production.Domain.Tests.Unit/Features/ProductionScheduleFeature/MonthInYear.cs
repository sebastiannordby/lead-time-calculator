using LeadTimeCalculator.Production.Domain.Models.Schedule;

namespace LeadTimeCalculator.Production.Domain.Tests.Unit.Features.ProductionScheduleFeature
{
    public class OrderIdTests
    {
        [Fact]
        public void Id_with_same_number_equal()
        {
            // Given
            var orderIdOne = new ProductionOrderId(12);
            var orderIdTwo = new ProductionOrderId(12);

            // Then
            Assert.Equal(orderIdOne, orderIdTwo);
        }
    }
}
