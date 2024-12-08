using LeadTimeCalculator.Production.Domain.Exceptions;
using LeadTimeCalculator.Production.Domain.Models.Order;
using LeadTimeCalculator.Production.Domain.Models.Schedule;

namespace LeadTimeCalculator.Production.Domain.Tests.Unit.Schedule
{
    public class ProducableProductTests
    {
        [Fact]
        public void Adding_existing_parts_errors()
        {
            // Given
            var product = new ProducableProduct(
                "Robot Head",
                new ProductionTime(21),
                new ProductType("Robotics"));

            var part = new ProducableProductPart("" +
                "Robotic Head Eyelashes 2x");
            product.AddPart(part);

            // When & Then
            Assert.Throws<ProducableProductAddExistingPartException>(() =>
            {
                product.AddPart(part);
            });
        }
    }
}
