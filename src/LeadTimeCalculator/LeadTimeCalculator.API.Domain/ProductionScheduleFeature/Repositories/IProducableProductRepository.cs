using LeadTimeCalculator.API.Domain.ProductionScheduleFeature.Models;

namespace LeadTimeCalculator.API.Domain.ProductionScheduleFeature.Repositories
{
    public interface IProducableProductRepository
    {
        Task<ProducableProduct> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task SaveAsync(
            ProducableProduct producableProduct,
            CancellationToken cancellationToken = default);
    }
}
