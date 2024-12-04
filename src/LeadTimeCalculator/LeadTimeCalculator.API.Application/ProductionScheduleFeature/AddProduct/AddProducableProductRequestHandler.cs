using FluentValidation;
using LeadTimeCalculator.API.Constracts.ProductionScheduleFeature.AddProducableProduct;
using LeadTimeCalculator.API.Domain.ProductionScheduleFeature.Models;
using LeadTimeCalculator.API.Domain.ProductionScheduleFeature.Repositories;

namespace LeadTimeCalculator.API.Application.ProductionScheduleFeature.AddProduct
{
    public sealed class AddProducableProductRequestHandler
    {
        private readonly IValidator<AddProducableProductRequest> _requestValidator;
        private readonly IProducableProductRepository _producableProductRepository;

        public AddProducableProductRequestHandler(
            IValidator<AddProducableProductRequest> requestValidator,
            IProducableProductRepository producableProductRepository)
        {
            _requestValidator = requestValidator;
            _producableProductRepository = producableProductRepository;
        }

        public async Task HandleAsync(
            AddProducableProductRequest request,
            CancellationToken cancellationToken = default)
        {
            await _requestValidator
                .ValidateAndThrowAsync(request, cancellationToken);

            if (await _producableProductRepository.ExistsByNameAsync(request.ProductName))
                throw new ValidationException("Product already exists");

            var producableProduct = new ProducableProduct(
                request.ProductName,
                new ProductionTime(request.WorkdaysToProduce),
                new ProductType(request.ProductType));

            foreach (var part in request.Parts)
            {
                producableProduct.AddPart(
                    new ProducableProductPart(part));
            }

            await _producableProductRepository
                .SaveAsync(producableProduct, cancellationToken);
        }
    }
}
