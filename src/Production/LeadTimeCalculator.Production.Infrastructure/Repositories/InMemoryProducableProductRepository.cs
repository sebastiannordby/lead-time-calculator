using LeadTimeCalculator.Production.Application.ProductionScheduleFeature.Repositories;
using LeadTimeCalculator.Production.Domain.Models.Order;

namespace LeadTimeCalculator.API.Repositories
{
    internal sealed class InMemoryProducableProductRepository : IProducableProductRepository
    {
        private readonly List<ProducableProduct> _producableProducts = new();

        public async Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var exists = _producableProducts.Any(x => x.Name == name);

            return await Task.FromResult(exists);
        }

        public async Task<ProducableProduct> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var product = _producableProducts.First(x => x.Name == name);

            return await Task.FromResult(product);
        }

        public async Task SaveAsync(
            ProducableProduct producableProduct,
            CancellationToken cancellationToken = default)
        {
            if (!await ExistsByNameAsync(producableProduct.Name))
            {
                _producableProducts.Add(producableProduct);
            }
        }
    }
}
