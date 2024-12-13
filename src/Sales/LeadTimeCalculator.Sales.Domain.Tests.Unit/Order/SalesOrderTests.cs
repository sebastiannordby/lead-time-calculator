using Bogus;
using FluentAssertions;
using LeadTimeCalculator.Sales.Domain.Order;

namespace LeadTimeCalculator.Sales.Domain.Tests.Unit.Order
{
    public class SalesOrderTests
    {
        private readonly Faker _faker = new();

        [Theory]
        [InlineData("Aurélie Éléanor Caprille")]
        [InlineData("James Dallengar")]
        [InlineData("Bjørn Bærgenåsen Østker")]
        public void Create_order_sets_customer(
            string customer)
        {
            // When
            var sut = SalesOrder
                .CreateOrder(1, customer);

            // Then
            Assert.Equal(customer, sut.Customer);
        }


        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "")]
        [InlineData(-1, "    ")]
        public void Create_order_fails_on_invalid_input(
            int orderNumber, string? customer)
        {
            // When & Then
            Assert.ThrowsAny<Exception>(() =>
            {
                SalesOrder.CreateOrder(orderNumber, customer!);
            });
        }

        [Fact]
        public void Add_product_creates_orderline()
        {
            // Given
            var order = GetOrder("Aerodyamics LTD");
            var productDetails = new ProductDetails(
                productId: Guid.NewGuid(),
                productName: "Airplane Wing",
                quantity: new(ProductQuantity.QuantityType.Whole, 1),
                price: 123.45d);

            // When
            order.AddProduct(productDetails);

            // Then
            order.OrderLines
                 .Should()
                 .Contain(x =>
                    x.ProductId == productDetails.ProductId
                    && x.ProductName == productDetails.ProductName
                    && x.Quantity == productDetails.Quantity
                    && x.Price == productDetails.Price);
        }

        private SalesOrder GetOrder(string customer)
        {
            return SalesOrder.CreateOrder(1, customer);
        }
    }
}
