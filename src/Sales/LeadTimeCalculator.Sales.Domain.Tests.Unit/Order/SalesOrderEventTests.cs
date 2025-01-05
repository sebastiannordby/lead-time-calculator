using Bogus;
using FluentAssertions;
using LeadTimeCalculator.Sales.Domain.Order;
using LeadTimeCalculator.Sales.Domain.Order.Events;

namespace LeadTimeCalculator.Sales.Domain.Tests.Unit.Order
{
    public class SalesOrderEventTests
    {
        private readonly Faker _faker = new();

        [Fact]
        public void Add_product_replays_event()
        {
            // Given
            var addProductEvent = new AddProductEvent(
                productId: Guid.NewGuid(),
                productName: _faker.Random.Word(),
                quantity: new(ProductQuantity.QuantityType.Partial, 1.2),
                price: _faker.Random.Double(-1000, 1000));

            // When
            var order = new SalesOrder([addProductEvent]);

            // Then
            order.UncommittedEvents
                .Should()
                .BeEmpty();
            order.OrderLines
                 .Should()
                 .Contain(x =>
                    x.ProductId == addProductEvent.ProductId
                    && x.ProductName == addProductEvent.ProductName
                    && x.Price == addProductEvent.Price);
        }

        [Fact]
        public void Add_product_stores_event()
        {
            // Given
            var order = new SalesOrder([
                new OrderCreatedEvent(1, "Aerodynamics LTD")
            ]);

            var productDetails = new ProductDetails(
                productId: Guid.NewGuid(),
                productName: "Airplane Wing",
                quantity: new(ProductQuantity.QuantityType.Whole, 1),
                price: 123.45d);

            // When
            order.AddProduct(productDetails);

            // Then
            var uncommittedEvents = order.UncommittedEvents;

            uncommittedEvents.Count.Should().Be(1);
            var productAddedEvent = uncommittedEvents.OfType<AddProductEvent>().Single();
            productAddedEvent.ProductId.Should().Be(productDetails.ProductId);
            productAddedEvent.ProductName.Should().Be(productDetails.ProductName);
            productAddedEvent.Quantity.Should().Be(productDetails.Quantity);
            productAddedEvent.Price.Should().Be(productDetails.Price);
        }

        [Fact]
        public void Mark_as_finished_processing_replays_event()
        {
            // Given & When
            var order = new SalesOrder([
                new OrderCreatedEvent(
                    1, "Aerodynamics LTD"),
                new AddProductEvent(
                    productId: Guid.NewGuid(),
                    productName: _faker.Random.Word(),
                    quantity: new(ProductQuantity.QuantityType.Partial, 1.2),
                    price: _faker.Random.Double(-1000, 1000)),
                new OrderMarkedAsFinishedProcessingEvent()
            ]);

            // Then
            order.UncommittedEvents
                .Should()
                .BeEmpty();

            order.Status.Should().Be(SalesOrderStatus.FinishedProcessing);
        }

        [Fact]
        public void Mark_as_finished_processing_stores_event()
        {
            // Given
            var order = new SalesOrder([
                new OrderCreatedEvent(
                    1, "Aerodynamics LTD"),
                new AddProductEvent(
                    productId: Guid.NewGuid(),
                    productName: _faker.Random.Word(),
                    quantity: new(ProductQuantity.QuantityType.Partial, 1.2),
                    price: _faker.Random.Double(-1000, 1000))
            ]);

            // When
            order.MarkAsFinishedProcessing();

            // Then
            order.UncommittedEvents.Count.Should().Be(1);
            var productAddedEvent = order.UncommittedEvents.OfType<OrderMarkedAsFinishedProcessingEvent>().Single();
            productAddedEvent.Should().NotBeNull();
        }
    }
}
