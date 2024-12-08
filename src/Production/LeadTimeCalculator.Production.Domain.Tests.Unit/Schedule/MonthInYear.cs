using LeadTimeCalculator.Production.Domain.Models.Order;

namespace LeadTimeCalculator.Production.Domain.Tests.Unit.Schedule
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
