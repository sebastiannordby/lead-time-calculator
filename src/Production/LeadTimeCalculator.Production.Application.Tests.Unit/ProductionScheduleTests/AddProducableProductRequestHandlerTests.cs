using LeadTimeCalculator.Production.Application.ProductionScheduleFeature.Repositories;
using LeadTimeCalculator.Production.Application.ProductionScheduleFeature.UseCases.AddProduct;
using LeadTimeCalculator.Production.Contracts.Schedule.AddProducableProduct;
using LeadTimeCalculator.Production.Domain.Models.Order;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ValidationException = FluentValidation.ValidationException;

namespace LeadTimeCalculator.Production.Application.Tests.Unit.ProductionScheduleTests
{
    public class AddProducableProductRequestHandlerTests
    {
        private readonly AddProducableProductRequestHandler _sut;
        private readonly IProducableProductRepository _producableProductRepositoryMock;

        public AddProducableProductRequestHandlerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddProductionApplicationFeature();

            _producableProductRepositoryMock = Substitute
                .For<IProducableProductRepository>();
            serviceCollection.AddSingleton(_producableProductRepositoryMock);

            var services = serviceCollection.BuildServiceProvider();
            _sut = services.GetRequiredService<AddProducableProductRequestHandler>();
        }

        [Theory]
        [InlineData(null!, null!, 0)]
        [InlineData("", "", -1)]
        public async Task InvalidRequest_ErrorsWithInvalidFields(
            string productName,
            string productType,
            double workdaysToProduce)
        {
            // Guven
            var invalidRequest = new AddProducableProductRequest(
                productName,
                productType,
                workdaysToProduce,
                Enumerable.Empty<string>());

            // When & Then
            var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _sut.HandleAsync(invalidRequest);
            });

            Assert.Contains(nameof(invalidRequest.ProductName),
                ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(nameof(invalidRequest.WorkdaysToProduce),
                ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(nameof(invalidRequest.ProductType),
                ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Errors_when_product_already_exists()
        {
            // Given
            var requestMatchingExistingProduct = new AddProducableProductRequest(
                "Lenovo X1 Thinkpad", "Computer", 1.2, Enumerable.Empty<string>());

            _producableProductRepositoryMock
                .ExistsByNameAsync(requestMatchingExistingProduct.ProductName)
                .Returns(true);

            // When & Then
            var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _sut.HandleAsync(requestMatchingExistingProduct);
            });

            Assert.Contains("Product already exists",
                ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ValidRequest_PersistsProduct()
        {
            // Given
            var validRequest = new AddProducableProductRequest(
                ProductName: "Lenovo X1 Thinkpad",
                ProductType: "Computer",
                WorkdaysToProduce: 1.2,
                Parts: ["CPU", "GPU", "PowerSupply"]);

            // When
            await _sut.HandleAsync(validRequest);

            // Then
            await _producableProductRepositoryMock
                .Received(1)
                .SaveAsync(
                    Arg.Is<ProducableProduct>(x =>
                        x.Name == validRequest.ProductName
                        && x.ProductType.Name == validRequest.ProductType
                        && x.ProductionTime.Workdays == validRequest.WorkdaysToProduce
                        && x.Parts.Count == validRequest.Parts.Count()
                        && x.Parts.All(part => validRequest.Parts.Contains(part.Name))));
        }
    }
}
