using LeadTimeCalculator.API.Domain.ProductionScheduleFeature.Models;

namespace LeadTimeCalculator.API.Application.ProductionScheduleFeature
{
    public interface IProductionScheduleRepository
    {
        Task SaveAsync(
            ProductionSchedule productionSchedule,
            CancellationToken cancellationToken);

        Task FindAsync(
            ProductType productType,
            CancellationToken cancellationToken);
    }
}
