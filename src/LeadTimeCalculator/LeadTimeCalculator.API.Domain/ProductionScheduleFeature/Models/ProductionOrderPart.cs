namespace LeadTimeCalculator.API.Domain.ProductionScheduleFeature.Models
{
    public sealed record ProductionOrderPart(
        string Name,
        DateTime? ExpectedArrivalDate);
}
