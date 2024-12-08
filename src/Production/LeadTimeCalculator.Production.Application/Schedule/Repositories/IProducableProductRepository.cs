using LeadTimeCalculator.Production.Domain.Models.Order;

namespace LeadTimeCalculator.Production.Application.ProductionScheduleFeature.Repositories
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
